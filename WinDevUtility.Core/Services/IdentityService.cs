﻿using Microsoft.Identity.Client;
using System;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using WinDevUtility.Core.Helpers;

namespace WinDevUtility.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly string _clientId = ConfigurationManager.AppSettings["IdentityClientId"];
        private readonly string[] _graphScopes = new string[] { "user.read" };
        private bool _integratedAuthAvailable;
        private IPublicClientApplication _client;
        private AuthenticationResult _authenticationResult;

        public event EventHandler LoggedIn;

        public event EventHandler LoggedOut;

        public void InitializeWithAadAndPersonalMsAccounts()
        {
            _integratedAuthAvailable = false;
            _client = PublicClientApplicationBuilder.Create(_clientId)
                                                    .WithAuthority(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount)
                                                    .Build();
        }

        public void InitializeWithAadMultipleOrgs(bool integratedAuth = false)
        {
            _integratedAuthAvailable = integratedAuth;
            _client = PublicClientApplicationBuilder.Create(_clientId)
                                                    .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                                                    .Build();
        }

        public void InitializeWithAadSingleOrg(string tenant, bool integratedAuth = false)
        {
            _integratedAuthAvailable = integratedAuth;
            _client = PublicClientApplicationBuilder.Create(_clientId)
                                                    .WithAuthority(AzureCloudInstance.AzurePublic, tenant)
                                                    .Build();
        }

        public bool IsLoggedIn() => _authenticationResult != null;

        public async Task<LoginResultType> LoginAsync()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return LoginResultType.NoNetworkAvailable;
            }

            try
            {
                var accounts = await _client.GetAccountsAsync().ConfigureAwait(false);
                _authenticationResult = await _client.AcquireTokenInteractive(_graphScopes)
                                                     .WithAccount(accounts.FirstOrDefault())
                                                     .ExecuteAsync();

                LoggedIn?.Invoke(this, EventArgs.Empty);
                return LoginResultType.Success;
            }
            catch (MsalClientException ex)
            {
                if (ex.ErrorCode == "authentication_canceled")
                {
                    return LoginResultType.CancelledByUser;
                }

                return LoginResultType.UnknownError;
            }
            catch (Exception)
            {
                return LoginResultType.UnknownError;
            }
        }

        public bool IsAuthorized() => true;

        public string GetAccountUserName()
        {
            return _authenticationResult?.Account?.Username;
        }

        public async Task LogoutAsync()
        {
            try
            {
                var accounts = await _client.GetAccountsAsync();
                var account = accounts.FirstOrDefault();
                if (account != null)
                {
                    await _client.RemoveAsync(account);
                }

                _authenticationResult = null;
                LoggedOut?.Invoke(this, EventArgs.Empty);
            }
            catch (MsalException)
            {
            }
        }

        public async Task<string> GetAccessTokenForGraphAsync() => await GetAccessTokenAsync(_graphScopes);

        private async Task<string> GetAccessTokenAsync(string[] scopes)
        {
            var acquireTokenSuccess = await AcquireTokenSilentAsync(scopes);
            if (acquireTokenSuccess)
            {
                return _authenticationResult.AccessToken;
            }
            else
            {
                try
                {
                    // Interactive authentication is required
                    var accounts = await _client.GetAccountsAsync();
                    _authenticationResult = await _client.AcquireTokenInteractive(scopes)
                                                         .WithAccount(accounts.FirstOrDefault())
                                                         .ExecuteAsync();
                    return _authenticationResult.AccessToken;
                }
                catch (MsalException)
                {
                    // AcquireTokenSilent and AcquireTokenInteractive failed, the session will be closed.
                    _authenticationResult = null;
                    LoggedOut?.Invoke(this, EventArgs.Empty);
                    return string.Empty;
                }
            }
        }

        public async Task<bool> AcquireTokenSilentAsync() => await AcquireTokenSilentAsync(_graphScopes);

        private async Task<bool> AcquireTokenSilentAsync(string[] scopes)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return false;
            }

            try
            {
                var accounts = await _client.GetAccountsAsync();
                _authenticationResult = await _client.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                                     .ExecuteAsync();
                return true;
            }
            catch (MsalUiRequiredException)
            {
                if (_integratedAuthAvailable)
                {
                    try
                    {
                        _authenticationResult = await _client.AcquireTokenByIntegratedWindowsAuth(scopes)
                                                             .ExecuteAsync();
                        return true;
                    }
                    catch (MsalUiRequiredException)
                    {
                        // Interactive authentication is required
                        return false;
                    }
                }
                else
                {
                    // Interactive authentication is required
                    return false;
                }
            }
            catch (MsalException)
            {
                return false;
            }
        }
    }
}

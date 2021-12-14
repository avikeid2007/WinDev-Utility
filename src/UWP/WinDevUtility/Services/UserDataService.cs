using System;
using System.Threading.Tasks;
using WinDevUtility.Core.Models;
using WinDevUtility.Core.Services;
using WinDevUtility.Helpers;
using WinDevUtility.ViewModels;

using Windows.Storage;

namespace WinDevUtility.Services
{
    public class UserDataService : IUserDataService
    {
        private const string _userSettingsKey = "IdentityUser";
        private UserViewModel _user;
        private readonly IIdentityService _identityService;
        private readonly IMicrosoftGraphService _microsoftGraphService;

        public event EventHandler<UserViewModel> UserDataUpdated;

        public UserDataService(IIdentityService identityService, IMicrosoftGraphService microsoftGraphService)
        {
            _identityService = identityService;
            _microsoftGraphService = microsoftGraphService;
        }

        public void Initialize()
        {
            _identityService.LoggedIn += OnLoggedInAsync;
            _identityService.LoggedOut += OnLoggedOutAsync;
        }

        public async Task<UserViewModel> GetUserAsync()
        {
            return _user ??= await GetUserFromCacheAsync() ?? GetDefaultUserData();
        }

        private async void OnLoggedInAsync(object sender, EventArgs e)
        {
            _user = await GetUserFromGraphApiAsync();
            UserDataUpdated?.Invoke(this, _user);
        }

        private async void OnLoggedOutAsync(object sender, EventArgs e)
        {
            _user = null;
            await ApplicationData.Current.LocalFolder.SaveAsync<User>(_userSettingsKey, null);
        }

        private async Task<UserViewModel> GetUserFromCacheAsync()
        {
            var cacheData = await ApplicationData.Current.LocalFolder.ReadAsync<User>(_userSettingsKey);
            return await GetUserViewModelFromDataAsync(cacheData);
        }

        private async Task<UserViewModel> GetUserFromGraphApiAsync()
        {
            var accessToken = await _identityService.GetAccessTokenForGraphAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            var userData = await _microsoftGraphService.GetUserInfoAsync(accessToken);
            if (userData != null)
            {
                userData.Photo = await _microsoftGraphService.GetUserPhoto(accessToken);
                await ApplicationData.Current.LocalFolder.SaveAsync(_userSettingsKey, userData);
            }

            return await GetUserViewModelFromDataAsync(userData);
        }

        private async Task<UserViewModel> GetUserViewModelFromDataAsync(User userData)
        {
            if (userData == null)
            {
                return null;
            }

            var userPhoto = string.IsNullOrEmpty(userData.Photo)
                ? ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
                : await ImageHelper.ImageFromStringAsync(userData.Photo);

            return new UserViewModel()
            {
                Name = userData.DisplayName,
                UserPrincipalName = userData.UserPrincipalName,
                Photo = userPhoto
            };
        }

        private UserViewModel GetDefaultUserData()
        {
            return new UserViewModel()
            {
                Name = _identityService.GetAccountUserName(),
                Photo = ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
            };
        }
    }
}

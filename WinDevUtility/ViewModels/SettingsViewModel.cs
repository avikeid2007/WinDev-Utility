using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Core.Helpers;
using WinDevUtility.Core.Services;
using WinDevUtility.Helpers;
using WinDevUtility.Services;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace WinDevUtility.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private IUserDataService _userDataService;
        private IIdentityService _identityService;
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        private DelegateCommand _logInCommand;
        private DelegateCommand _logOutCommand;
        private bool _isLoggedIn;
        private bool _isBusy;
        private UserViewModel _user;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                return _switchThemeCommand ??= new DelegateCommand<object>(
                        async (param) =>
                        {
                            ElementTheme = (ElementTheme)param;
                            await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
                        });
            }
        }

        public DelegateCommand LogInCommand => _logInCommand ??= new DelegateCommand(OnLogInAsync, () => !IsBusy);

        public DelegateCommand LogOutCommand => _logOutCommand ??= new DelegateCommand(OnLogOutAsync, () => !IsBusy);

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                LogInCommand.RaiseCanExecuteChanged();
                LogOutCommand.RaiseCanExecuteChanged();
            }
        }

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public SettingsViewModel(IIdentityService identityService, IUserDataService userDataService)
        {
            _identityService = identityService;
            _userDataService = userDataService;
        }

        public async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            IsLoggedIn = _identityService.IsLoggedIn();
            User = await _userDataService.GetUserAsync();
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await InitializeAsync();
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            _identityService.LoggedIn -= OnLoggedIn;
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private async void OnLogInAsync()
        {
            IsBusy = true;
            var loginResult = await _identityService.LoginAsync();
            if (loginResult != LoginResultType.Success)
            {
                await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                IsBusy = false;
            }
        }

        private async void OnLogOutAsync()
        {
            IsBusy = true;
            await _identityService.LogoutAsync();
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsBusy = false;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            User = null;
            IsLoggedIn = false;
            IsBusy = false;
        }
    }
}

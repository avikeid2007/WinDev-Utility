using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WinDevUtility.Core.Helpers;
using WinDevUtility.Core.Services;
using WinDevUtility.Helpers;
using WinDevUtility.Services;
using WinDevUtility.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace WinDevUtility.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private static INavigationService _navigationService;
        private Frame _frame;
        private WinUI.NavigationView _navigationView;
        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;
        private UserViewModel _user;
        private bool _isBusy;
        private bool _isLoggedIn;
        private bool _isAuthorized;
        private IIdentityService _identityService;
        private IUserDataService _userDataService;

        public ICommand ItemInvokedCommand { get; }

        public ICommand LoadedCommand { get; }

        public DelegateCommand UserProfileCommand { get; }

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                UserProfileCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { SetProperty(ref _isAuthorized, value); }
        }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ShellViewModel(INavigationService navigationServiceInstance, IIdentityService identityService, IUserDataService userDataService)
        {
            _navigationService = navigationServiceInstance;
            _identityService = identityService;
            _userDataService = userDataService;
            LoadedCommand = new DelegateCommand(OnLoadedAsync);
            UserProfileCommand = new DelegateCommand(OnUserProfileAsync);
            ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView)
        {
            _frame = frame;
            _navigationView = navigationView;
            _frame.NavigationFailed += (_, e) => throw e.Exception;
            _frame.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
        }

        private async void OnLoadedAsync()
        {
            IsLoggedIn = _identityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            User = await _userDataService.GetUserAsync();
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            IsBusy = false;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            User = null;
            IsLoggedIn = false;
            IsAuthorized = false;
            CleanRestrictedPagesFromNavigationHistory();
            GoBackToLastUnrestrictedPage();
        }

        private void CleanRestrictedPagesFromNavigationHistory()
        {
            var app = App.Current as App;
            foreach (var pageToken in PageTokens.GetAll())
            {
                var page = app.GetPage(pageToken);
                var isRestricted = Attribute.IsDefined(page, typeof(Restricted));
                if (isRestricted)
                {
                    _navigationService.RemoveAllPages(pageToken);
                }
            }
        }

        private void GoBackToLastUnrestrictedPage()
        {
            var currentPage = _frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                _navigationService.GoBack();
            }
        }

        private async void OnUserProfileAsync()
        {
            if (IsLoggedIn)
            {
                _navigationService.Navigate(PageTokens.SettingsPage, null);
            }
            else
            {
                IsBusy = true;
                var loginResult = await _identityService.LoginAsync();
                if (loginResult != LoginResultType.Success)
                {
                    await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                    IsBusy = false;
                }
            }
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.Navigate("Settings", null);
                return;
            }

            if (args.InvokedItemContainer is WinUI.NavigationViewItem selectedItem)
            {
                var pageKey = selectedItem.GetValue(NavHelper.NavigateToProperty) as string;
                _navigationService.Navigate(pageKey, null);
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack();
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            var selectedItem = GetSelectedItem(_navigationView.MenuItems, e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            _navigationService.GoBack();
        }

        private WinUI.NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<WinUI.NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var sourcePageKey = sourcePageType.Name;
            sourcePageKey = sourcePageKey.Substring(0, sourcePageKey.Length - 4);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == sourcePageKey;
        }
    }
}

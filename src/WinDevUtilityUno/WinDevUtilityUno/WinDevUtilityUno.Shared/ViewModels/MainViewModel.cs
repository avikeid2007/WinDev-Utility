using BasicMvvm;
using BasicMvvm.Commands;

using System.Windows.Input;

using WinDevUtilityUno.Views;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using WinUI = Microsoft.UI.Xaml.Controls;



namespace WinDevUtilityUno.ViewModels
{
    class MainViewModel : BindableBase
    {
        public Frame Frame { get; internal set; }
        public ICommand ItemInvokedCommand => new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
#if WINDOWS_UWP
                // Set theme for window root.
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.ActualTheme == ElementTheme.Dark)
                    {
                        frameworkElement.RequestedTheme = ElementTheme.Light;
                    }
                    else
                    {
                        frameworkElement.RequestedTheme = ElementTheme.Dark;
                    }
                }
#else
        Uno.UI.ApplicationHelper.RequestedCustomTheme = "Dark";
#endif
            }

            if (args.InvokedItemContainer is WinUI.NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag)
                {
                    case "poco":
                        Frame.Navigate(typeof(PocoPage), null, new EntranceNavigationTransitionInfo());
                        break;
                    case "guid":
                        Frame.Navigate(typeof(GuidPage), null, new EntranceNavigationTransitionInfo());
                        break;
                    case "xaml":
                        Frame.Navigate(typeof(XamlPage), null, new EntranceNavigationTransitionInfo());
                        break;
                    case "cmd":
                        Frame.Navigate(typeof(CommandPage), null, new EntranceNavigationTransitionInfo());
                        break;
                    default:
                        break;
                }
                //Frame.Navigate(typeof(PocoPage), null, new EntranceNavigationTransitionInfo());
            }
        }
    }
}

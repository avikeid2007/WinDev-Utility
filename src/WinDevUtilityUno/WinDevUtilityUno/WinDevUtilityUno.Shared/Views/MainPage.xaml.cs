using Microsoft.Extensions.DependencyInjection;

using WinDevUtilityUno.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinDevUtilityUno.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        internal MainViewModel VM { get; }
        public MainPage()
        {
            this.InitializeComponent();
            var container = ((App)App.Current).Container;
            // Request an instance of the ViewModel and set it to the DataContext
            VM = (MainViewModel)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(MainViewModel));
            DataContext = VM;
        }
        private void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
            {
                VM.Frame = shellFrame;
                shellFrame.Navigate(typeof(PocoPage), null, new EntranceNavigationTransitionInfo());
            }
        }
    }
}

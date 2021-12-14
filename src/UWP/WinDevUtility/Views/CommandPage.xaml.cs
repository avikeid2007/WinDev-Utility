
using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class CommandPage : Page
    {
        public CommandPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TxtPocoInput.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }
    }
}

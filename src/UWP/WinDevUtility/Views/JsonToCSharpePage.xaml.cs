using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinDevUtility.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JsonToCSharpePage : Page
    {
        public JsonToCSharpePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TxtJsonInput.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }
    }
}

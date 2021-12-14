using Microsoft.Extensions.DependencyInjection;

using System.Linq;

using WinDevUtilityUno.ViewModels;

using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinDevUtilityUno.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuidPage : Page
    {
        public GuidPage()
        {
            this.InitializeComponent();
            var container = ((App)App.Current).Container;
            // Request an instance of the ViewModel and set it to the DataContext
            VM = (GuidViewModel)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(GuidViewModel));
            DataContext = VM;
        }

        public GuidViewModel VM { get; }
        private void TextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}

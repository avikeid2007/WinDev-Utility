
using WinDevUtility.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class PocoPage : Page
    {
        private PocoViewModel ViewModel => DataContext as PocoViewModel;

        public PocoPage()
        {
            InitializeComponent();
        }
    }
}

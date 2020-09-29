
using WinDevUtility.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class BlogsPage : Page
    {
        public BlogsViewModel ViewModel => DataContext as BlogsViewModel;

        public BlogsPage()
        {
            InitializeComponent();
        }

        private void ContentPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

using System;

using WinDevUtility.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class BlogsPage : Page
    {
        private BlogsViewModel ViewModel => DataContext as BlogsViewModel;

        public BlogsPage()
        {
            InitializeComponent();
        }
    }
}

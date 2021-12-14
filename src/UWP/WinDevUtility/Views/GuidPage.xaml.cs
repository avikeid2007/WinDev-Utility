using System;

using WinDevUtility.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class GuidPage : Page
    {
        private GuidViewModel ViewModel => DataContext as GuidViewModel;

        public GuidPage()
        {
            InitializeComponent();
        }
    }
}

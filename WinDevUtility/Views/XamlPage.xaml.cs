using System;

using WinDevUtility.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class XamlPage : Page
    {
        private XamlViewModel ViewModel => DataContext as XamlViewModel;

        public XamlPage()
        {
            InitializeComponent();
        }
    }
}

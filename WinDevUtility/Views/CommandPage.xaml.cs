using System;

using WinDevUtility.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WinDevUtility.Views
{
    public sealed partial class CommandPage : Page
    {
        private CommandViewModel ViewModel => DataContext as CommandViewModel;

        public CommandPage()
        {
            InitializeComponent();
        }
    }
}

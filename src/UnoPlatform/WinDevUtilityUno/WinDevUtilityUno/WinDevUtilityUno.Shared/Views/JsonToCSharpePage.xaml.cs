﻿using Microsoft.Extensions.DependencyInjection;
using WinDevUtilityUno.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinDevUtilityUno.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JsonToCSharpePage : Page
    {
        public JsonToCSharpePage()
        {
            this.InitializeComponent();
            var container = ((App)App.Current).Container;
            // Request an instance of the ViewModel and set it to the DataContext
            VM = (JsonToCSharpeViewModel)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(JsonToCSharpeViewModel));
            DataContext = VM;
        }
        public JsonToCSharpeViewModel VM { get; }

    }
}

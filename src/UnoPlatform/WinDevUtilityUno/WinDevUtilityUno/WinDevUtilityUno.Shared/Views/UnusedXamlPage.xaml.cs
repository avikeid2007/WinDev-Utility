using Microsoft.Extensions.DependencyInjection;
using WinDevUtilityUno.Models;
using WinDevUtilityUno.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinDevUtilityUno.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnusedXamlPage : Page
    {
        public UnusedXamlPage()
        {
            this.InitializeComponent();
            var container = ((App)App.Current).Container;
            // Request an instance of the ViewModel and set it to the DataContext
            VM = (UnusedXamlViewModel)ActivatorUtilities.GetServiceOrCreateInstance(container, typeof(UnusedXamlViewModel));
            DataContext = VM;
        }
        public UnusedXamlViewModel VM { get; }
        private void dg_loadingRowGroup(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            if (group.GroupItems[0] is DataResource item)
            {
                e.RowGroupHeader.PropertyValue = item.DefinedInXamlFile.FileName;
            }
        }
    }
}

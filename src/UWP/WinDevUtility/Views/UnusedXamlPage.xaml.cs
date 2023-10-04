using WinDevUtility.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinDevUtility.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnusedXamlPage : Page
    {
        public UnusedXamlPage()
        {
            this.InitializeComponent();
        }

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

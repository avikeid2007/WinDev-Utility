using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using WinDevUtility.Views;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace WinDevUtility.Services
{
    public class FirstRunDisplayService : IFirstRunDisplayService
    {
        private static bool shown;
        public async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.IsFirstRun && !shown)
                    {
                        shown = true;
                        var dialog = new FirstRunDialog();
                        await dialog.ShowAsync();
                    }
                });
        }
    }
}

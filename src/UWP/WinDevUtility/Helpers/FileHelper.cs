using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace WinDevUtility.Helpers
{
    public static class FileHelper
    {
        public static void CopyText(string text)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
        public static async Task<bool> SaveFileAsync(string text, FileTypes fileTypes, string fileName = "")
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add(GetFiletypes(fileTypes));
            savePicker.SuggestedFileName = fileName;
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteTextAsync(file, text);
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    return true;
                }
            }
            return false;
        }

        private static KeyValuePair<string, IList<string>> GetFiletypes(FileTypes fileTypes)
        {
            return fileTypes switch
            {
                FileTypes.CS => new KeyValuePair<string, IList<string>>
                (
                        "CS File",
                        new List<string>() { ".cs" }
               ),
                FileTypes.Txt => new KeyValuePair<string, IList<string>>
                (
                        "Text File",
                        new List<string>() { ".txt" }
               ),
                FileTypes.Xaml => new KeyValuePair<string, IList<string>>
                (
                        "XAML File",
                        new List<string>() { ".xaml" }
                ),
                _ => new KeyValuePair<string, IList<string>>
                (
                        "Text File",
                        new List<string>() { ".txt" }
                )
            };
        }
    }
}

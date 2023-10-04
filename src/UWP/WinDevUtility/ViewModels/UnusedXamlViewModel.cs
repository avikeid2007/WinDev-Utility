using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Helpers;
using WinDevUtility.Models;
using WinDevUtility.Services;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
namespace WinDevUtility.ViewModels
{
    class UnusedXamlViewModel : ViewModelBase
    {
        private List<XamlFile> _xamlFiles;
        private StorageFolder _projectfolder;
        private List<string> _projectPath;
        private readonly IDialogService _dialogService;
        private bool _isGrouped;
        private bool _isUnusedOnly;
        public bool IsGrouped
        {
            get => _isGrouped;
            set
            {
                _isGrouped = value;
            }
        }
        public bool IsUnusedOnly
        {
            get => _isUnusedOnly;
            set
            {
                _isUnusedOnly = value;
            }
        }
        public List<string> ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                RaisePropertyChanged();
            }
        }
        public List<DataResource> ApplicationResources { get; set; }
        public UnusedXamlViewModel(IDialogService dialogService)
        {
            ApplicationResources = new List<DataResource>();
            _dialogService = dialogService;
        }
        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            _ = SettingsStorageExtensions.SaveSettingAsync(PageTokens.UnusedXamlPage, nameof(PageTokens));
        }
        public ICommand FindCommand => new AsyncCommand(OnFindCommandExecuteAsync);
        public ICommand CopyCommand => new DelegateCommand(OnCopyCommandExecute);
        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);
        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);
        public ICommand KeyDownCommand => new AsyncCommand<KeyRoutedEventArgs>(OnKeyDownCommandExecuteAsync);
        public ICommand SelectProjectCommand => new AsyncCommand<KeyRoutedEventArgs>(OnSelectProjectCommandExecuteAsync);
        public ICommand DgLoadedCommand => new DelegateCommand<object>(OnDgLoadedCommandExecuteAsync);

        private void OnDgLoadedCommandExecuteAsync(object arg)
        {
            if (arg is DataGrid dataGrid)
            {
                _dataGrid = dataGrid;
            }
        }

        private async Task OnSelectProjectCommandExecuteAsync(KeyRoutedEventArgs arg)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };
            folderPicker.FileTypeFilter.Add("*");
            _projectfolder = await folderPicker.PickSingleFolderAsync();
            ProjectPath = _projectfolder?.Path.Split('\\').ToList();
        }
        List<DataResource> filterResources = null;
        private async Task OnFindCommandExecuteAsync()
        {
            try
            {
                if (_projectfolder == null)
                {
                    return;
                }
                ApplicationResources?.Clear();
                IsLoading = true;
                _xamlFiles = new List<XamlFile>();
                await LoadFilesAsync(_projectfolder);
                foreach (var xamlFile in _xamlFiles)
                {
                    DetermineStyles(xamlFile);
                }
                foreach (var rec in ApplicationResources)
                {
                    foreach (var xamlFile in _xamlFiles)
                    {
                        var regexToUse = $@"{{StaticResource[ ]{{1,}}(ResourceKey=){{0,1}}{rec.Key}" +
                                             $@"|{{ThemeResource[ ]{{1,}}(ResourceKey=){{0,1}}{rec.Key}";
                        xamlFile.Count = new Regex(regexToUse).Matches(xamlFile.XAML).Count;
                        if (xamlFile.Count > 0)
                        {
                            rec.UsedInXamlFiles.Add(xamlFile);
                        }
                    }
                }
                SetData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void SetData()
        {
            if (ApplicationResources?.Count > 0)
            {
                _dataGrid.ItemsSource = null;
                filterResources = new List<DataResource>(ApplicationResources.Where(x => !IsUnusedOnly || !x.UsedInXamlFiles.Any()));
                if (IsGrouped)
                {
                    _dataGrid.ItemsSource = GroupResult(filterResources).View;
                }
                else if (filterResources != null)
                {
                    _dataGrid.ItemsSource = filterResources;
                }
            }
        }

        private CollectionViewSource GroupResult(List<DataResource> resources)
        {

            ObservableCollection<GroupInfoCollection<DataResource>> xamResource = new ObservableCollection<GroupInfoCollection<DataResource>>();
            var query = from item in resources
                        group item by item.DefinedInXamlFile.FileName into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoCollection<DataResource> info = new GroupInfoCollection<DataResource>
                {
                    Key = g.GroupName
                };
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                xamResource.Add(info);
            }
            CollectionViewSource groupedItems = new CollectionViewSource
            {
                IsSourceGrouped = true,
                Source = xamResource
            };
            return groupedItems;
        }
        private async Task OnKeyDownCommandExecuteAsync(KeyRoutedEventArgs obj)
        {
            if (obj.Key == VirtualKey.F5)
            {
                await OnFindCommandExecuteAsync();
            }
            if (obj.Key == VirtualKey.F6)
            {
                OnCopyCommandExecute();
            }
            if (obj.Key == VirtualKey.F7)
            {
                await OnExportCommandExecuteAsync();
            }
            if (obj.Key == VirtualKey.Escape)
            {
                OnClearCommandExecute();
            }
        }
        private void OnClearCommandExecute()
        {
            _dataGrid.ItemsSource = null;
            filterResources = null;
            ApplicationResources?.Clear();
        }
        private async Task OnExportCommandExecuteAsync()
        {
            if (filterResources?.Count > 0)
            {
                var content = CsvExportHelper.Export(filterResources.Select(x => new
                {
                    x.Key,
                    x.DefinedInXamlFile.FileName,
                    x.Occurrences
                }).ToList(), true);
                await FileHelper.SaveFileAsync(content, FileTypes.Csv, "unusedXamlResource");
            }
        }

        private void OnCopyCommandExecute()
        {
            if (filterResources?.Count > 0)
            {
                var content = CsvExportHelper.Export(filterResources.Select(x => new
                {
                    x.Key,
                    x.DefinedInXamlFile.FileName,
                    x.Occurrences
                }).ToList(), true);
                FileHelper.CopyText(content);
            }
        }
        private async Task LoadFilesAsync(StorageFolder folder)
        {
            foreach (StorageFile file in await folder.GetFilesAsync())
            {
                if (file.FileType == ".xaml")
                {
                    _xamlFiles.Add(new XamlFile
                    {
                        Path = file.Name,
                        XAML = await LoadXAMLAsync(file),
                    });
                }
            }
            var folders = await folder.GetFoldersAsync();
            if (folders.Count > 0)
            {
                foreach (StorageFolder subfolder in folders)
                {
                    await LoadFilesAsync(subfolder);
                }
            }
        }
        async Task<string> LoadXAMLAsync(StorageFile file)
        {
            return await FileIO.ReadTextAsync(file);
        }

        private void DetermineStyles(XamlFile xamlFile)
        {
            int pos = xamlFile.XAML.IndexOf("x:Key=", 0);
            while (pos > -1)
            {
                int end = xamlFile.XAML.IndexOf("\"", pos + 7);
                var key = xamlFile.XAML.Substring(pos + 7, end - pos - 7);
                if (!ApplicationResources.Any(x => x.Key == key && x.DefinedInXamlFile.FileName == xamlFile.FileName))
                {
                    ApplicationResources.Add(new DataResource { DefinedInXamlFile = xamlFile, Key = key, UsedInXamlFiles = new List<XamlFile>() });
                }
                pos = xamlFile.XAML.IndexOf("x:Key=", end);
            }
        }
        public int UnusedCount
        {
            get
            {
                return ApplicationResources.Count(x => x.UsedInXamlFiles.Count == 0);
            }
        }
        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { _progress = value; RaisePropertyChanged(); }
        }
        private bool _isLoading;
        private DataGrid _dataGrid;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(); }
        }
    }
}

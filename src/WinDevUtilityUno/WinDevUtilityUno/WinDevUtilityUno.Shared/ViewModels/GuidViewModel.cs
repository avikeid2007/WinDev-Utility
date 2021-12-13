using BasicMvvm;
using BasicMvvm.Commands;

using SequentialGuid;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using WinDevUtilityUno.Constants;
using WinDevUtilityUno.Extensions;

using WinDevUtilityUno.Helpers;

using Windows.Storage;

using Windows.System;

using Windows.UI.Xaml.Input;

namespace WinDevUtilityUno.ViewModels
{
    public class GuidViewModel : BindableBase
    {
        private bool _isUpper;
        private bool _isRemoveHyphens;
        private bool _isIncludeBraces;
        private bool _isSequential;
        private int _noofGuid;
        private string _guidText;
        private ObservableCollection<string> _guidCollection;
        private bool? _isValidGuid;
        private int _selectGuidOption;
        private string _validGuidText;
        public ICommand GeneratePropertiesCommand => new DelegateCommand(OnGeneratePropertiesCommandExecute);
        public ICommand CopyAllCommand => new DelegateCommand(OnCopyAllCommandExecute);
        public ICommand CopySingleCommand => new DelegateCommand(OnCopySingleCommandExecute);
        public ICommand CopyCommand => new DelegateCommand<Object>(OnCopyCommandExecute);
        public ICommand CutCommand => new DelegateCommand<Object>(OnCutCommandExecute);
        public ICommand DeleteCommand => new DelegateCommand<Object>(OnDeleteCommandExecute);
        public ICommand RefreshCommand => new DelegateCommand(OnRefreshCommandExecute);
        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);
        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);
        public ICommand KeyDownCommand => new AsyncCommand<KeyRoutedEventArgs>(OnKeyDownCommandExecuteAsync);

        private async Task OnKeyDownCommandExecuteAsync(KeyRoutedEventArgs obj)
        {
            if (obj.Key == VirtualKey.F5)
            {
                OnGeneratePropertiesCommandExecute();
            }
            if (obj.Key == VirtualKey.F6)
            {
                OnCopyAllCommandExecute();
            }
            if (obj.Key == VirtualKey.F7)
            {
                await OnExportCommandExecuteAsync();
            }
            if (obj.Key == VirtualKey.F8)
            {
                OnClearCommandExecute();
            }
        }
        public GuidViewModel()
        {
            StartUpSettingAsync().AwaitAsync(() => GuidText = GenerateGuid(), null);
            NoofGuid = 1;
        }
        //public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        //{
        //    _ = SettingsStorageExtensions.SaveSettingAsync(PageTokens.GuidPage, nameof(PageTokens));
        //}
        public bool IsUpper
        {
            get { return _isUpper; }
            set
            {
                if (_isUpper != value)
                {
                    _isUpper = value;

                    OnPropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsRemoveHyphens
        {
            get { return _isRemoveHyphens; }
            set
            {
                if (_isRemoveHyphens != value)
                {
                    _isRemoveHyphens = value;
                    OnPropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsIncludeBraces
        {
            get { return _isIncludeBraces; }
            set
            {
                if (_isIncludeBraces != value)
                {
                    _isIncludeBraces = value;
                    OnPropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsSequential
        {
            get { return _isSequential; }
            set
            {
                if (_isSequential != value)
                {
                    _isSequential = value;
                    OnPropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public int NoofGuid
        {
            get { return _noofGuid; }
            set
            {
                if (_noofGuid != value)
                {
                    _noofGuid = value;
                    OnPropertyChanged();
                }
            }
        }
        public string GuidText
        {
            get { return _guidText; }
            set
            {
                if (_guidText != value)
                {
                    _guidText = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<string> GuidCollection
        {
            get { return _guidCollection; }
            set
            {
                if (_guidCollection != value)
                {
                    _guidCollection = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool? IsValidGuid
        {
            get { return _isValidGuid; }
            set
            {
                if (_isValidGuid != value)
                {
                    _isValidGuid = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ValidGuidText
        {
            get { return _validGuidText; }
            set
            {
                _validGuidText = value;
                IsValidGuid = (bool?)(string.IsNullOrEmpty(value) ? (ValueType)null : Guid.TryParse(_validGuidText, out var _));
                OnPropertyChanged();
            }
        }
        public int SelectGuidOption
        {
            get { return _selectGuidOption; }
            set
            {
                _selectGuidOption = value;
                GuidCollection?.Clear();
                OnPropertyChanged();
            }
        }
        private void OnClearCommandExecute()
        {
            GuidCollection?.Clear();
        }
        private void OnGeneratePropertiesCommandExecute()
        {
            if (NoofGuid > 0)
            {
                (GuidCollection??=new ObservableCollection<string>()).Clear();
                for (int i = 0; i <= NoofGuid - 1; i++)
                {
                    string guid = GenerateGuid();
                    GuidCollection.Add(guid);
                }
            }
        }

        private string GenerateGuid()
        {
            string guid = IsSequential ? SequentialGuidGenerator.Instance.NewGuid().ToString() : Guid.NewGuid().ToString();
            if (IsUpper)
            {
                guid = guid.ToUpper();
            }
            if (IsRemoveHyphens)
            {
                guid = guid.Replace("-", "");
            }
            if (IsIncludeBraces)
            {
                guid = $"{{{guid}}}";
            }
            return guid;
        }
        private async Task StartUpSettingAsync()
        {
            IsUpper = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsUpper));
            IsIncludeBraces = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsIncludeBraces));
            IsRemoveHyphens = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsRemoveHyphens));
            IsSequential = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsSequential));
        }

        private async Task OnExportCommandExecuteAsync()
        {
            await FileHelper.SaveFileAsync(GetCollectionText(), FileTypes.Txt, "GuidFile.txt");
        }
        private void OnCopyAllCommandExecute()
        {
            FileHelper.CopyText(GetCollectionText());
        }

        private string GetCollectionText()
        {
            var guidcollectionText = string.Empty;
            GuidCollection?.ToList().ForEach(x => guidcollectionText += $"{x}\r");
            return guidcollectionText;
        }

        private void OnDeleteCommandExecute(object obj)
        {
            if (obj is string guid && !string.IsNullOrEmpty(guid))
            {
                GuidCollection.Remove(guid);
            }
        }
        private void OnCutCommandExecute(object obj)
        {
            if (obj is string guid && !string.IsNullOrEmpty(guid))
            {
                FileHelper.CopyText(guid);
                GuidCollection.Remove(guid);
            }
        }

        private void OnCopyCommandExecute(object obj)
        {
            if (obj is string guid && !string.IsNullOrEmpty(guid))
            {
                FileHelper.CopyText(guid);
            }
        }
        private void OnCopySingleCommandExecute()
        {
            if (!string.IsNullOrEmpty(GuidText))
            {
                FileHelper.CopyText(GuidText);
            }
        }
        private void OnRefreshCommandExecute() => GuidText = GenerateGuid();
    }
}

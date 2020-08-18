using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using SequentialGuid;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Helpers;
using Windows.Storage;

namespace WinDevUtility.ViewModels
{
    public class GuidViewModel : ViewModelBase
    {
        private bool _isUpper;
        private bool _isIncludeHyphens;
        private bool _isIncludeBraces;
        private bool _isSequential;
        private int _noofGuid;
        private string _guidText;
        private ObservableCollection<string> _guidCollection;
        private bool _isValidGuid;
        public ICommand GeneratePropertiesCommand => new DelegateCommand(OnGeneratePropertiesCommandExecute);
        public ICommand CopyCommand => new DelegateCommand(OnCopyCommandExecute);
        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);
        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);
        public GuidViewModel()
        {
            _ = StartUpSettingAsync();
            NoofGuid = 1;
            GuidText = GenerateGuid();
        }

        public bool IsUpper
        {
            get { return _isUpper; }
            set
            {
                if (_isUpper != value)
                {
                    _isUpper = value;

                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsIncludeHyphens
        {
            get { return _isIncludeHyphens; }
            set
            {
                if (_isIncludeHyphens != value)
                {
                    _isIncludeHyphens = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsValidGuid
        {
            get { return _isValidGuid; }
            set
            {
                if (_isValidGuid != value)
                {
                    _isValidGuid = value;
                    RaisePropertyChanged();
                }
            }
        }
        private int _selectGuidOption;

        public int SelectGuidOption
        {
            get { return _selectGuidOption; }
            set
            {
                _selectGuidOption = value;
                RaisePropertyChanged();
            }
        }
        private void OnClearCommandExecute()
        {
            throw new NotImplementedException();
        }
        private void OnGeneratePropertiesCommandExecute()
        {
            if (NoofGuid > 0)
            {
                if (GuidCollection == null)
                {
                    GuidCollection = new ObservableCollection<string>();
                }
                GuidCollection.Clear();
                for (int i = 0; i < NoofGuid - 1; i++)
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
            if (!IsIncludeHyphens)
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
            IsIncludeHyphens = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsIncludeHyphens));
            IsSequential = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsSequential));
        }

        private Task OnExportCommandExecuteAsync()
        {
            throw new NotImplementedException();
        }
        private void OnCopyCommandExecute()
        {
            throw new NotImplementedException();
        }
    }
}

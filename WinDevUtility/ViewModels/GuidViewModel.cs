using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinDevUtility.ViewModels
{
    public class GuidViewModel : ViewModelBase
    {
        private bool _isLower;
        private bool _isUpper;
        private bool _isIncludeHyphens;
        private bool _isIncludeBraces;
        private bool _isSequential;
        private int _noofGuid;
        private string _guidText;
        private ObservableCollection<string> _guidCollection;
        private string _emptyGuid;
        private bool _isValidGuid;
        public ICommand GeneratePropertiesCommand => new DelegateCommand(OnGeneratePropertiesCommandExecute);
        public ICommand CopyCommand => new DelegateCommand(OnCopyCommandExecute);
        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);
        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);
        public GuidViewModel()
        {
        }
        public bool IsLower
        {
            get { return _isLower; }
            set
            {
                if (_isLower != value)
                {
                    _isLower = value;
                    RaisePropertyChanged();
                }
            }
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
        public string EmptyGuid
        {
            get { return _emptyGuid; }
            set
            {
                if (_emptyGuid != value)
                {
                    _emptyGuid = value;
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
                    var guid = Guid.NewGuid().ToString();
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
                    GuidCollection.Add(guid);
                }
            }
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

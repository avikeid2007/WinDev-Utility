
using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Services;

namespace WinDevUtility.ViewModels
{
    public class XamlViewModel : ViewModelBase
    {
        private bool _isGridLayoutChecked;
        private bool _isListViewChecked;
        private bool _isStyleChecked;
        private bool _isClassToXamlChecked;
        private int _columnWidth;
        private int _columnType;
        private int _noofColumns;
        private int _rowHeight;
        private int _rowType;
        private int _noofRows;
        private string _outputText;
        private IDialogService _dialogService;
        private bool _isCustomHeight;
        private bool _isCustomWidth;
        public ICommand GeneratePropertiesCommand => new AsyncCommand(OnGeneratePropertiesCommandExecuteAsync);

        private async Task OnGeneratePropertiesCommandExecuteAsync()
        {
            if (IsGridLayoutChecked)
            {
                OutputText = await GenerateGridLayoutAsync();
            }
        }

        private async Task<string> GenerateGridLayoutAsync()
        {
            if (NoofRows == 0 || NoofColumns == 0)
            {
                await _dialogService.AlertAsync("invalid inputs");
            }
            throw new NotImplementedException();
        }

        public IEnumerable<string> TypeCollection
        {
            get
            {
                yield return "*";
                yield return "Auto";
                yield return "Custom";
            }
        }
        public ICommand CopyCommand => new DelegateCommand(OnCopyCommandExecute);

        private void OnCopyCommandExecute()
        {
            throw new NotImplementedException();
        }

        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);

        private Task OnExportCommandExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);

        private void OnClearCommandExecute()
        {
            throw new NotImplementedException();
        }

        public XamlViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            NoofColumns = 1;
            NoofRows = 1;
        }
        public string OutputText
        {
            get { return _outputText; }
            set
            {
                if (_outputText != value)
                {
                    _outputText = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ColumnWidth
        {
            get { return _columnWidth; }
            set
            {
                if (_columnWidth != value)
                {
                    _columnWidth = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ColumnType
        {
            get { return _columnType; }
            set
            {
                if (_columnType != value)
                {
                    _columnType = value;
                    SetCustomWidth(value);
                    RaisePropertyChanged();
                }
            }
        }

        private void SetCustomWidth(int value)
        {
            if (value == 2)
            {
                IsCustomWidth = true;
            }
            else
            {
                IsCustomWidth = false;
            }
        }
        private void SetCustomHeight(int value)
        {
            if (value == 2)
            {
                IsCustomHeight = true;
            }
            else
            {
                IsCustomHeight = false;
            }
        }
        public int NoofColumns
        {
            get { return _noofColumns; }
            set
            {
                if (_noofColumns != value)
                {
                    _noofColumns = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int RowHeight
        {
            get { return _rowHeight; }
            set
            {
                if (_rowHeight != value)
                {
                    _rowHeight = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int RowType
        {
            get { return _rowType; }
            set
            {
                if (_rowType != value)
                {
                    _rowType = value;
                    SetCustomHeight(value);
                    RaisePropertyChanged();
                }
            }
        }
        public int NoofRows
        {
            get { return _noofRows; }
            set
            {
                if (_noofRows != value)
                {
                    _noofRows = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsGridLayoutChecked
        {
            get { return _isGridLayoutChecked; }
            set
            {
                if (_isGridLayoutChecked != value)
                {
                    _isGridLayoutChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsListViewChecked
        {
            get { return _isListViewChecked; }
            set
            {
                if (_isListViewChecked != value)
                {
                    _isListViewChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsStyleChecked
        {
            get { return _isStyleChecked; }
            set
            {
                if (_isStyleChecked != value)
                {
                    _isStyleChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsClassToXamlChecked
        {
            get { return _isClassToXamlChecked; }
            set
            {
                if (_isClassToXamlChecked != value)
                {
                    _isClassToXamlChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsCustomHeight
        {
            get { return _isCustomHeight; }
            set
            {
                if (_isCustomHeight != value)
                {
                    _isCustomHeight = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsCustomWidth
        {
            get { return _isCustomWidth; }
            set
            {
                if (_isCustomWidth != value)
                {
                    _isCustomWidth = value;
                    RaisePropertyChanged();
                }
            }
        }

    }
}

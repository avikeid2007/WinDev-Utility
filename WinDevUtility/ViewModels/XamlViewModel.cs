
using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Helpers;
using WinDevUtility.Services;

namespace WinDevUtility.ViewModels
{
    public class XamlViewModel : ViewModelBase
    {
        private const string GridTextstr = @"<Grid>
<Grid.RowDefinitions>
{0}</Grid.RowDefinitions>
<Grid.ColumnDefinitions>
{1}</Grid.ColumnDefinitions>
</Grid>";
        private const string RowTextstr = @"<RowDefinition Height=""{0}"" />";
        private const string ColumnTextstr = @"<ColumnDefinition Width=""{0}"" />";
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
            var rowText = string.Empty;
            for (int i = 0; i < NoofRows; i++)
            {
                rowText += string.Format(RowTextstr, GetRowType(RowType)) + "\r";
            }
            var colText = string.Empty;
            for (int i = 0; i < NoofRows; i++)
            {
                colText += string.Format(ColumnTextstr, GetColumnType(ColumnType)) + "\r";
            }
            return string.Format(GridTextstr, rowText, colText);
        }

        private string GetRowType(int rowType)
        {
            return rowType switch
            {
                0 => "*",
                1 => "auto",
                2 => RowHeight.ToString(),
                _ => "*"
            };
        }
        private string GetColumnType(int colType)
        {
            return colType switch
            {
                0 => "*",
                1 => "auto",
                2 => ColumnWidth.ToString(),
                _ => "*"
            };
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
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
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
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
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
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
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
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
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

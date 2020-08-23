
using AsyncCommands;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Constants;
using WinDevUtility.Extensions;
using WinDevUtility.Helpers;
using WinDevUtility.Services;
using Windows.Storage;

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
        private string _inputText;
        private bool _isUseGrid;
        private bool _isUseTextBox;
        private bool _isUseTwoWayBinding;
        private bool _isUseXBind;
        private bool _isUWP;
        private bool _isWPF;
        private bool _isXamarin;
        public ICommand GeneratePropertiesCommand => new AsyncCommand(OnGeneratePropertiesCommandExecuteAsync);

        private async Task OnGeneratePropertiesCommandExecuteAsync()
        {
            if (!IsGridLayoutChecked && !IsClassToXamlChecked)
            {
                await _dialogService.AlertAsync("Please select any option");
            }
            if (IsGridLayoutChecked)
            {
                OutputText = await GenerateGridLayoutAsync(NoofRows, NoofColumns, RowType, ColumnType);
            }
            if (IsClassToXamlChecked)
            {
                OutputText = await GenerateClassToXamlAsync();
            }
        }
        private int i = 0;
        private async Task<string> GenerateClassToXamlAsync()
        {
            i = 0;
            if (!string.IsNullOrEmpty(InputText))
            {
                if (await ValidateCommandInputesAsync())
                {
                    return "";
                }
                var lines = await InputText.GetLinesAsync();
                if (lines?.Count() > 0)
                {
                    return await GenerateXamlTextAsync(lines.ToArray());
                }
            }
            else
            {
                await _dialogService.AlertAsync("Enter the input text");
            }
            return "";
        }
        private string RemoveWantedkeyword(string text)
        {
            return text.Replace(" virtual ", " ").Replace(" readonly ", " ").Trim();
        }
        string bindingText = string.Empty;
        private async Task<string> GenerateXamlTextAsync(string[] vs)
        {
            bindingText = IsUseXBind ? "x:Bind" : "Binding";
            bool isClassFound = false;
            string xamlText = string.Empty;
            for (int i = 0; i < vs.Length; i++)
            {
                if (isClassFound && vs[i].Contains("public "))
                {
                    var text = vs[i].RemoveExtraWhiteSpace().Trim();
                    if (!string.IsNullOrEmpty(text))
                    {
                        string line = RemoveWantedkeyword(text);
                        string[] lineWords = line.Split(new char[0]);
                        if (lineWords.Length == 3)
                        {
                            xamlText += $"{GetControlText(GetQualifiedTypes(lineWords[1]), lineWords[2])}\r";
                        }
                    }
                }
                if (!isClassFound && vs[i].Contains("class "))
                {
                    isClassFound = true;
                }
            }
            if (!string.IsNullOrEmpty(xamlText))
            {
                if (IsUseGrid)
                {
                    return await GenerateGridLayoutAsync(noofRows: i, noofColumns: 1, content: xamlText);
                }
                else
                {
                    if (IsXamarin)
                    {
                        return string.Format(Globals.StackTextXamarinstr, xamlText);
                    }
                    return string.Format(Globals.StackTextstr, xamlText);
                }
            }
            return string.Empty;
        }

        private string GetControlText(string type, string name)
        {
            if (type.Equals(nameof(DateTimeOffset), StringComparison.OrdinalIgnoreCase) || type.Equals(nameof(DateTime), StringComparison.OrdinalIgnoreCase))
            {
                return GetDateControl(name);
            }
            if (type.Equals(nameof(Guid), StringComparison.OrdinalIgnoreCase))
            {
                return GetComboControl(name);
            }
            if (type.Equals(nameof(Boolean), StringComparison.OrdinalIgnoreCase) || type.Equals("bool", StringComparison.OrdinalIgnoreCase))
            {
                return GetCheckBoxControl(name);
            }
            if (type.StartsWith(nameof(List), StringComparison.OrdinalIgnoreCase)
                || type.StartsWith("ObservableCollection", StringComparison.OrdinalIgnoreCase)
                || type.StartsWith(nameof(IEnumerable), StringComparison.OrdinalIgnoreCase)
                || type.StartsWith(nameof(IList), StringComparison.OrdinalIgnoreCase)
                || type.StartsWith(nameof(ICollection), StringComparison.OrdinalIgnoreCase))
            {
                return GetListViewControl(name);
            }
            else
            {
                return GetTextControl(name);
            }
        }
        private string SetMode(string name)
        {
            if (IsUseTwoWayBinding)
            {
                return $"{name}, Mode=TwoWay";
            }
            return name;
        }
        private string GetListViewControl(string name)
        {
            return string.Format(Globals.ListViewControlstr, bindingText, name, SetGridRow());
        }

        private string GetCheckBoxControl(string name)
        {
            return string.Format(Globals.CheckBoxControlstr, bindingText, SetMode(name), SetGridRow());
        }

        private string GetTextControl(string name)
        {
            if (IsXamarin)
            {
                if (IsUseTextBox)
                    return string.Format(Globals.TextBoxControlXamarinstr, bindingText, SetMode(name), SetGridRow());
                else
                    return string.Format(Globals.TextBlockControlXamarinstr, bindingText, SetMode(name), SetGridRow());
            }
            else
            {
                if (IsUseTextBox)
                    return string.Format(Globals.TextBoxControlstr, bindingText, SetMode(name), SetGridRow());
                else
                    return string.Format(Globals.TextBlockControlstr, bindingText, SetMode(name), SetGridRow());
            }
        }

        private string GetComboControl(string name)
        {
            if (IsXamarin)
            {
                return string.Format(Globals.ComboControlXamarinstr, bindingText, SetMode(name), SetGridRow());
            }
            else
            {
                return string.Format(Globals.ComboControlstr, bindingText, SetMode(name), SetGridRow());
            }
        }

        private string SetGridRow()
        {
            var row = IsUseGrid ? string.Format(@"Grid.Row=""{0}""", i) : string.Empty;
            i++;
            return row;
        }

        private string GetDateControl(string name)
        {
            if (IsWPF)
            {
                return string.Format(Globals.DateControlWPFstr, bindingText, SetMode(name), SetGridRow());
            }
            else
            {
                return string.Format(Globals.DateControlstr, bindingText, SetMode(name), SetGridRow());
            }

        }
        private string GetQualifiedTypes(string type)
        {
            if (type.StartsWith("Nullable"))
            {
                type = type.Replace("Nullable<", "").Replace(">", "").Trim();
            }
            if (type.EndsWith("?"))
            {
                type = type.Replace("?", "").Trim();
            }
            if (type.StartsWith("System."))
            {
                type = type.Replace("System.", "").Trim();
            }
            return type.ToFirstUpper();
        }

        private async Task<bool> ValidateCommandInputesAsync()
        {
            if (!InputText.Trim().Contains("class "))
            {
                await _dialogService.AlertAsync("Invalid Class");
                return true;
            }
            return false;
        }

        private async Task<string> GenerateGridLayoutAsync(int noofRows, int noofColumns, int rowType = 0, int columnType = 0, string content = "")
        {
            if (noofRows == 0 || noofColumns == 0)
            {
                await _dialogService.AlertAsync("invalid inputs");
            }
            var rowText = string.Empty;
            for (int i = 0; i < noofRows; i++)
            {
                rowText += string.Format(Globals.RowTextstr, GetRowType(rowType)) + "\r";
            }
            var colText = string.Empty;
            for (int i = 0; i < noofColumns; i++)
            {
                colText += string.Format(Globals.ColumnTextstr, GetColumnType(columnType)) + "\r";
            }
            return string.Format(Globals.GridTextstr, rowText, colText, content);
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
        private async Task StartUpSettingAsync()
        {
            IsGridLayoutChecked = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsGridLayoutChecked));
            //IsListViewChecked = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsListViewChecked));
            //IsStyleChecked = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsStyleChecked));
            IsClassToXamlChecked = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsClassToXamlChecked));
            IsUseXBind = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsUseXBind));
            IsUseTwoWayBinding = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsUseTwoWayBinding));
            IsUseTextBox = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsUseTextBox));
            IsUseGrid = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsUseGrid));
            IsUWP = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsUWP));
            IsXamarin = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsXamarin));
            IsWPF = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsWPF));
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
            StartUpSettingAsync().AwaitAsync(() => Init(), null);
        }

        private void Init()
        {
            if (!IsGridLayoutChecked && !IsClassToXamlChecked)
            {
                IsGridLayoutChecked = true;
            }
            if (!IsUWP && !IsWPF && !IsXamarin)
            {
                IsUWP = true;
            }
        }

        public string InputText
        {
            get { return _inputText; }
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    RaisePropertyChanged();
                }
            }
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
        public bool IsUseGrid
        {
            get { return _isUseGrid; }
            set
            {
                if (_isUseGrid != value)
                {
                    _isUseGrid = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsUseTextBox
        {
            get { return _isUseTextBox; }
            set
            {
                if (_isUseTextBox != value)
                {
                    _isUseTextBox = value;
                    if (!value)
                    {
                        IsUseTwoWayBinding = false;
                    }
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsUseTwoWayBinding
        {
            get { return _isUseTwoWayBinding; }
            set
            {
                if (_isUseTwoWayBinding != value)
                {
                    _isUseTwoWayBinding = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsUseXBind
        {
            get { return _isUseXBind; }
            set
            {
                if (_isUseXBind != value)
                {
                    _isUseXBind = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }

            }
        }
        public bool IsUWP
        {
            get { return _isUWP; }
            set
            {
                if (_isUWP != value)
                {
                    _isUWP = value; if (!value)
                    {
                        IsUseXBind = false;
                    }
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsWPF
        {
            get { return _isWPF; }
            set
            {
                if (_isWPF != value)
                {
                    _isWPF = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsXamarin
        {
            get { return _isXamarin; }
            set
            {
                if (_isXamarin != value)
                {
                    _isXamarin = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }

    }
}

using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Constants;
using WinDevUtility.Extensions;
using WinDevUtility.Helpers;
using WinDevUtility.Services;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace WinDevUtility.ViewModels
{
    public class PocoViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private string PrivatePropertyString = string.Empty;
        private string PublicPropertyString = string.Empty;
        private const string privateStr = "private";
        private const string publicStr = "public";
        private bool _isincludePropertyChange;
        private bool _isPrism;
        private bool _isDirtyCheck;
        private bool _isGenerateClass;
        private string _inputText;
        private string _outputText;
        private string _className;
        private string _namespace;
        private string _baseClass;
        public ICommand GeneratePropertiesCommand => new AsyncCommand(OnGeneratePropertiesCommandExecuteAsync);
        public ICommand CopyCommand => new DelegateCommand(OnCopyCommandExecute);
        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);
        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);
        public ICommand KeyDownCommand => new AsyncCommand<KeyRoutedEventArgs>(OnKeyDownCommandExecuteAsync);
        private async Task OnKeyDownCommandExecuteAsync(KeyRoutedEventArgs obj)
        {
            if (obj.Key == VirtualKey.F5)
            {
                await OnGeneratePropertiesCommandExecuteAsync();
            }
            if (obj.Key == VirtualKey.F6)
            {
                OnCopyCommandExecute();
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
        public PocoViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _ = StartUpSettingAsync();
        }
        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            _ = SettingsStorageExtensions.SaveSettingAsync(PageTokens.POCOPage, nameof(PageTokens));
        }

        public string PlaceHolder => Globals.PocoPlaceHolder;

        public bool IsIncludePropertyChange
        {
            get { return _isincludePropertyChange; }
            set
            {
                if (_isincludePropertyChange != value)
                {
                    _isincludePropertyChange = value;
                    if (!value)
                    {
                        IsPrism = false;
                    }
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsPrism
        {
            get { return _isPrism; }
            set
            {
                if (_isPrism != value)
                {
                    _isPrism = value;
                    if (value)
                    {
                        IsDirtyCheck = true;
                    }
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsDirtyCheck
        {
            get { return _isDirtyCheck; }
            set
            {
                if (_isDirtyCheck != value)
                {
                    _isDirtyCheck = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsGenerateClass
        {
            get { return _isGenerateClass; }
            set
            {
                _isGenerateClass = value;
                RaisePropertyChanged();
            }
        }
        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                RaisePropertyChanged();
            }
        }
        public string OutputText
        {
            get { return _outputText; }
            set
            {
                _outputText = value;
                RaisePropertyChanged();
            }
        }
        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                RaisePropertyChanged();
            }
        }
        public string Namespace
        {
            get { return _namespace; }
            set
            {
                _namespace = value;
                RaisePropertyChanged();
            }
        }
        public string BaseClass
        {
            get { return _baseClass; }
            set
            {
                _baseClass = value;
                RaisePropertyChanged();
            }
        }
        private string classBaseText = "using System;\rusing System.Collections.Generic;\rusing System.Linq;\rusing System.Text;\r";

        private async Task OnGeneratePropertiesCommandExecuteAsync()
        {
            try
            {
                OutputText = string.Empty;
                PrivatePropertyString = string.Empty;
                PublicPropertyString = string.Empty;
                if (!string.IsNullOrEmpty(InputText))
                {
                    if (IsGenerateClass && await ValidateClassAsync())
                    {
                        return;
                    }
                    var lines = await InputText.GetLinesAsync();
                    if (lines?.Count() > 0)
                    {
                        OutputText = GenrateProperties(lines.ToArray());
                    }
                }
                else
                {
                    await _dialogService.AlertAsync("Enter the input text");
                }
            }
            catch
            {
                await _dialogService.AlertAsync("oops, something Went Wrong");
            }
        }
        private async Task<bool> ValidateClassAsync()
        {
            if (string.IsNullOrEmpty(ClassName) || (!string.IsNullOrEmpty(ClassName) && ClassName.Contains(" ")))
            {
                await _dialogService.AlertAsync("Enter a valid class name");
                return true;
            }
            if (!string.IsNullOrEmpty(BaseClass) && BaseClass.Contains(" "))
            {
                await _dialogService.AlertAsync("Enter a valid Base class name");
                return true;
            }
            if (!string.IsNullOrEmpty(Namespace) && Namespace.Contains(" "))
            {
                await _dialogService.AlertAsync("Enter a valid Namespace");
                return true;
            }
            return false;
        }
        private async Task StartUpSettingAsync()
        {
            IsIncludePropertyChange = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsIncludePropertyChange));
            IsPrism = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsPrism));
            IsDirtyCheck = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsDirtyCheck));
        }
        private string PropertyGetter(string propertyName) => "{ \r\tget { return " + propertyName + "; }\r";
        private string RemoveWantedkeyword(string text) => text.Replace(" virtual ", " ").Replace(" readonly ", " ").Trim();
        private string ValidatePropertyName(string propertyName)
        {
            propertyName = propertyName.StartsWith("_") ? propertyName.Substring(1) : propertyName;
            propertyName = propertyName.EndsWith(";") ? propertyName.TrimEnd(';') : propertyName;
            return propertyName;
        }
        private string GenrateProperties(string[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var text = properties[i].RemoveExtraWhiteSpace().Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    string line = RemoveWantedkeyword(text);
                    if (!line.Contains(":") || !line.Contains("\""))
                    {
                        string[] lineWords = line.Split(new char[0]);
                        string propertType;
                        string propertyName;
                        if (line.StartsWith("public", System.StringComparison.OrdinalIgnoreCase) || line.StartsWith("private", System.StringComparison.OrdinalIgnoreCase) || line.StartsWith("protected", System.StringComparison.OrdinalIgnoreCase))
                        {
                            propertyName = ValidatePropertyName(lineWords[2]);
                            propertType = lineWords[1];
                        }
                        else
                        {
                            propertyName = ValidatePropertyName(lineWords[1]);
                            propertType = lineWords[0];
                        }
                        SetPropertyStings(propertyName, propertType);
                    }
                }
            }
            var propertiesText = PrivatePropertyString + "\r\n" + PublicPropertyString;
            if (IsGenerateClass)
            {
                return GetClassText(propertiesText);
            }
            return propertiesText;
        }

        private string GetClassText(string propertiesText)
        {
            var classText = $"public class  {ClassName} {(string.IsNullOrEmpty(BaseClass) ? string.Empty : ": " + BaseClass)}\r{{\r{propertiesText}\r}}";
            if (!string.IsNullOrEmpty(Namespace))
            {
                classBaseText += $"namespace {Namespace}\r{{\r{classText}\r}}";
                return classBaseText;
            }
            return classText;
        }

        private void SetPropertyStings(string propertyName, string propertType)
        {
            var publicPropertyName = propertyName.ToFirstUpper();
            var privatePropertyName = $"_{ propertyName.ToFirstLower() }";
            PrivatePropertyString += $"{privateStr} {propertType} {privatePropertyName};\r";
            PublicPropertyString += $"\r{publicStr} {propertType} {publicPropertyName} \r";
            PublicPropertyString += PropertyGetter(privatePropertyName);
            PublicPropertyString += "\tset\r \t{\r";
            PublicPropertyString += IsDirtyCheck && !IsPrism ? "\t  if (" + privatePropertyName + "!= value)\r\t  {\r" : string.Empty;
            PublicPropertyString += IsPrism ? $"\t    SetProperty(ref {privatePropertyName}, value);\r" : $"\t    {privatePropertyName} = value;\r";
            PublicPropertyString += IsIncludePropertyChange && !IsPrism ? "\t    RaisePropertyChanged();\r" : string.Empty;
            PublicPropertyString += IsDirtyCheck && !IsPrism ? "\t  }\r" : string.Empty;
            PublicPropertyString += "\t} \r}";
        }

        private void OnClearCommandExecute()
        {
            InputText = string.Empty;
            OutputText = string.Empty;
            ClassName = string.Empty;
            Namespace = string.Empty;
            BaseClass = string.Empty;
        }

        private async Task OnExportCommandExecuteAsync()
        {
            if (!string.IsNullOrEmpty(OutputText))
            {
                await FileHelper.SaveFileAsync(OutputText, FileTypes.CS, ClassName ?? "");
            }
        }

        private void OnCopyCommandExecute()
        {
            if (!string.IsNullOrEmpty(OutputText))
            {
                FileHelper.CopyText(OutputText);
            }
        }
    }
}

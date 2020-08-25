
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
    public class CommandViewModel : ViewModelBase
    {
        private IDialogService _dialogService;

        private bool _isRelayCommand;
        private bool _isDelegateCommand;
        private bool _isAsyncCommand;
        private bool _isCustomCommand;
        private bool _isIncludeExecute;
        private bool _isIncludeCanExecute;
        private bool _isIncludeParameter;
        private string _inputText;
        private string _outputText;
        private string _commandType;
        private string _parameterType;
        public string PlaceHolder => @"Enter command names i.e.
SaveCommand
DeleteCommand
ShowDialogCommand";
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
        public CommandViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            StartUpSettingAsync().AwaitAsync(() => Init(), null);
        }
        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            _ = SettingsStorageExtensions.SaveSettingAsync(PageTokens.CommandPage, nameof(PageTokens));
        }
        private void Init()
        {
            if (!IsAsyncCommand && !IsCustomCommand && !IsDelegateCommand && !IsRelayCommand)
            {
                IsDelegateCommand = true;
            }
        }

        public bool IsRelayCommand
        {
            get { return _isRelayCommand; }
            set
            {
                if (_isRelayCommand != value)
                {
                    _isRelayCommand = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsDelegateCommand
        {
            get { return _isDelegateCommand; }
            set
            {
                if (_isDelegateCommand != value)
                {
                    _isDelegateCommand = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsAsyncCommand
        {
            get { return _isAsyncCommand; }
            set
            {
                if (_isAsyncCommand != value)
                {
                    _isAsyncCommand = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsCustomCommand
        {
            get { return _isCustomCommand; }
            set
            {
                if (_isCustomCommand != value)
                {
                    _isCustomCommand = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsIncludeExecute
        {
            get { return _isIncludeExecute; }
            set
            {
                if (_isIncludeExecute != value)
                {
                    _isIncludeExecute = value;
                    if (!value)
                    {
                        IsIncludeCanExecute = false;
                    }
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsIncludeCanExecute
        {
            get { return _isIncludeCanExecute; }
            set
            {
                if (_isIncludeCanExecute != value)
                {
                    _isIncludeCanExecute = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value.ToString());
                }
            }
        }
        public bool IsIncludeParameter
        {
            get { return _isIncludeParameter; }
            set
            {
                if (_isIncludeParameter != value)
                {
                    _isIncludeParameter = value;
                    RaisePropertyChanged();
                }
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
        public string CommandType
        {
            get { return _commandType; }
            set
            {
                if (_commandType != value)
                {
                    _commandType = value;
                    RaisePropertyChanged();
                    _ = SettingsStorageExtensions.SaveSettingAsync(value);
                }
            }
        }
        public string ParameterType
        {
            get { return _parameterType; }
            set
            {
                if (_parameterType != value)
                {
                    _parameterType = value;
                    RaisePropertyChanged();
                }
            }
        }
        private async Task OnGeneratePropertiesCommandExecuteAsync()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (await ValidateCommandInputesAsync())
                {
                    return;
                }
                var lines = await InputText.GetLinesAsync();
                if (lines?.Count() > 0)
                {
                    OutputText = GenerateCommandText(lines.ToArray());
                }
            }
            else
            {
                await _dialogService.AlertAsync("Enter the input text");
            }
        }
        private string SetCommandParameter(string commandType) => IsIncludeParameter ? $"{commandType}<{ParameterType}>" : commandType;
        private string GenerateCommandText(string[] vs)
        {
            var commandText = string.Empty;
            var commandExecuteText = string.Empty;
            var commandCanExecuteText = string.Empty;
            for (int i = 0; i < vs.Length; i++)
            {
                var nameText = vs[i].RemoveExtraWhiteSpace().Trim();
                if (!string.IsNullOrEmpty(nameText) && !nameText.Contains(" "))
                {
                    if (IsRelayCommand)
                    {
                        var allText = GenarateText(SetCommandParameter(Globals.relayCommandStr), nameText.ToFirstUpper());
                        commandText += allText.Item1 + "\r";
                        commandExecuteText += allText.Item2 + "\r";
                        commandCanExecuteText += allText.Item3 + "\r";
                    }
                    if (IsDelegateCommand)
                    {
                        var allText = GenarateText(SetCommandParameter(Globals.delegateCommandStr), nameText.ToFirstUpper());
                        commandText += allText.Item1 + "\r";
                        commandExecuteText += allText.Item2 + "\r";
                        commandCanExecuteText += allText.Item3 + "\r";
                    }
                    if (IsAsyncCommand)
                    {
                        var allText = GenarateText(SetCommandParameter(Globals.AsyncCommandStr), nameText.ToFirstUpper());
                        commandText += allText.Item1 + "\r";
                        commandExecuteText += allText.Item2 + "\r";
                        commandCanExecuteText += allText.Item3 + "\r";
                    }
                    if (IsCustomCommand)
                    {
                        var allText = GenarateText(SetCommandParameter(CommandType), nameText.ToFirstUpper());
                        commandText += allText.Item1 + "\r";
                        commandExecuteText += allText.Item2 + "\r";
                        commandCanExecuteText += allText.Item3 + "\r";
                    }
                }
            }
            return $"{commandText}\r{commandExecuteText}\r{commandCanExecuteText}";
        }

        private (string, string, string) GenarateText(string commandStr, string commandName)
        {
            var text = $"{Globals.publicStr} {commandStr} {commandName}";
            if (!IsIncludeExecute && !IsIncludeCanExecute)
            {
                return ($"{text} {Globals.getSetstr}", string.Empty, string.Empty);
            }
            else
            {
                if (IsIncludeExecute && !IsIncludeCanExecute)
                {
                    text += $" => new {commandStr}( {string.Format(Globals.executeMethodstr, commandName)} );";
                    return (text, GetExecuteString(string.Format(Globals.executeMethodstr, commandName)), string.Empty);
                }
                else
                {
                    text += $" => new {commandStr}({string.Format(Globals.executeMethodstr, commandName)}, {string.Format(Globals.canExecuteMethodstr, commandName)});";
                    return (text, GetExecuteString(string.Format(Globals.executeMethodstr, commandName)), GetCanExecuteString(string.Format(Globals.canExecuteMethodstr, commandName)));
                }
            }
        }

        private string GetCanExecuteString(string methodName)
        {
            return string.Format(Globals.canExecuteMethodDefination, methodName, IsIncludeParameter ? $"{ParameterType} obj" : string.Empty);
        }

        private string GetExecuteString(string methodName)
        {
            return string.Format(IsAsyncCommand ? Globals.executeAsyncMethodDefination : Globals.executeMethodDefination, methodName, IsIncludeParameter ? $"{ParameterType} obj" : " ");
        }

        private async Task<bool> ValidateCommandInputesAsync()
        {
            if ((IsCustomCommand && string.IsNullOrEmpty(CommandType)) || (!string.IsNullOrEmpty(CommandType) && CommandType.Contains(" ")))
            {
                await _dialogService.AlertAsync("Enter a valid Command Type");
                return true;
            }
            return await ValidateCommandParameterAsync();
        }
        private async Task<bool> ValidateCommandParameterAsync()
        {
            if ((IsIncludeParameter && string.IsNullOrEmpty(ParameterType)) || (!string.IsNullOrEmpty(CommandType) && CommandType.Contains(" ")))
            {
                await _dialogService.AlertAsync("Enter a valid Parameter Type");
                return true;
            }
            return false;
        }
        private async Task StartUpSettingAsync()
        {
            IsAsyncCommand = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsAsyncCommand));
            IsRelayCommand = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsRelayCommand));
            IsDelegateCommand = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsDelegateCommand));
            IsCustomCommand = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsCustomCommand));
            IsIncludeExecute = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsIncludeExecute));
            IsIncludeCanExecute = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsIncludeCanExecute));
            CommandType = await ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(CommandType));
        }
        private void OnCopyCommandExecute()
        {
            if (!string.IsNullOrEmpty(OutputText))
            {
                FileHelper.CopyText(OutputText);
            }
        }
        private async Task OnExportCommandExecuteAsync()
        {
            if (!string.IsNullOrEmpty(OutputText))
            {
                await FileHelper.SaveFileAsync(OutputText, FileTypes.CS, "MyCommands");
            }
        }
        private void OnClearCommandExecute()
        {
            InputText = string.Empty;
            OutputText = string.Empty;
            ParameterType = string.Empty;
        }
    }
}

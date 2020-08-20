
using AsyncCommands;
using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Extensions;
using WinDevUtility.Services;

namespace WinDevUtility.ViewModels
{
    public class CommandViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private const string icommandStr = "ICommand";
        private const string relayCommandStr = "RelayCommand";
        private const string delegateCommandStr = "DelegateCommand";
        private const string AsyncCommandStr = "AsyncCommand";
        private const string publicStr = "public";
        private const string getSetstr = "{ get; set; }";
        private const string executeMethodstr = "On{0}Executed";
        private const string canExecuteMethodstr = "Can{0}Executed";
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
        public ICommand GeneratePropertiesCommand => new AsyncCommand(OnGeneratePropertiesCommandExecuteAsync);

        private async Task OnGeneratePropertiesCommandExecuteAsync()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (await ValidateCustomCommandAsync() && await ValidateCommandParameterAsync())
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
        private string SetCommandParameter(string commandType)
        {
            if (IsIncludeParameter)
            {
                return $"{commandType}<{ParameterType}>";
            }
            return commandType;
        }
        private string GenerateCommandText(string[] vs)
        {
            var commandText = string.Empty;
            for (int i = 0; i < vs.Length; i++)
            {
                var nameText = vs[i].RemoveExtraWhiteSpace().Trim();
                if (!string.IsNullOrEmpty(nameText) && !nameText.Contains(" "))
                {
                    if (IsRelayCommand)
                    {

                        commandText += GenarateText(SetCommandParameter(relayCommandStr), nameText.ToFirstUpper()) + ";\r";
                    }
                    if (IsDelegateCommand)
                    {
                        commandText += GenarateText(SetCommandParameter(delegateCommandStr), nameText.ToFirstUpper()) + ";\r";
                    }
                    if (IsAsyncCommand)
                    {
                        commandText += GenarateText(SetCommandParameter(AsyncCommandStr), nameText.ToFirstUpper()) + ";\r";
                    }
                    if (IsCustomCommand)
                    {
                        commandText += GenarateText(SetCommandParameter(CommandType), nameText.ToFirstUpper()) + ";\r";
                    }
                }
            }
            return commandText;
        }

        private string GenarateText(string commandStr, string commandName)
        {
            var text = $"{publicStr} {commandStr} {commandName}";
            if (!IsIncludeExecute && !IsIncludeCanExecute)
            {
                return $"{text} {getSetstr}";
            }
            else
            {
                if (IsIncludeExecute && !IsIncludeCanExecute)
                {
                    text += $" => new {commandStr}( {string.Format(executeMethodstr, commandName)} )";
                    return text;
                }
                else
                {
                    text += $" => new {commandStr}({string.Format(executeMethodstr, commandName)}, {string.Format(canExecuteMethodstr, commandName)})";
                    return text;
                }
            }
        }

        private async Task<bool> ValidateCustomCommandAsync()
        {
            if (IsCustomCommand && string.IsNullOrEmpty(CommandType) || (!string.IsNullOrEmpty(CommandType) && CommandType.Contains(" ")))
            {
                await _dialogService.AlertAsync("Enter a valid Command Type");
                return true;
            }
            return false;
        }
        private async Task<bool> ValidateCommandParameterAsync()
        {
            if (IsIncludeParameter && string.IsNullOrEmpty(ParameterType) || (!string.IsNullOrEmpty(CommandType) && CommandType.Contains(" ")))
            {
                await _dialogService.AlertAsync("Enter a valid Parameter Type");
                return true;
            }
            return false;
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

        public CommandViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
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

    }
}

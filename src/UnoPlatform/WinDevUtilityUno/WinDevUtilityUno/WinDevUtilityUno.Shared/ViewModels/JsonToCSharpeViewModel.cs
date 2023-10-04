using BasicMvvm;
using BasicMvvm.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUitiliy.JsonToCSharpe;
using WinDevUtilityUno.Constants;
using WinDevUtilityUno.Helpers;
using WinDevUtilityUno.Services;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace WinDevUtilityUno.ViewModels
{
    public class JsonToCSharpeViewModel : BindableBase
    {
        private bool _isUsePascal;
        private bool _isUseProperty;
        private bool _isUseNullableValues;
        private bool _isClass;
        private bool _isInpc;
        private bool _isRecord;
        private bool _isPrism;
        private bool _isImMutable;
        private bool _isAttributeAdded;
        private bool _isJsonPropertyAttribute;
        private string _inputText;
        private string _outputText;
        private bool _isMutable;
        private OutputCollectionType _selectedType;
        private readonly IDialogService _dialogService;

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }
        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged();
            }
        }
        public ICommand GeneratePropertiesCommand => new AsyncCommand(OnGeneratePropertiesCommandExecuteAsync);
        public ICommand ResetCommand => new DelegateCommand(OnResetCommandExecute);

        private void OnResetCommandExecute()
        {
            Setup();
        }

        private async Task OnGeneratePropertiesCommandExecuteAsync()
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                return;
            }
            JsonClassGenerator generator = new JsonClassGenerator();
            ConfigureGenerator(generator);
            try
            {
                var sb = generator.GenerateClasses(InputText, errorMessage: out string errorMessage);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await _dialogService.AlertAsync(errorMessage);
                }
                else
                {
                    OutputText = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.AlertAsync(ex.Message);
            }
        }
        public static IEnumerable<OutputCollectionType> CollectionTypes => Enum.GetValues(typeof(OutputCollectionType)).Cast<OutputCollectionType>().ToList();
        private void Setup()
        {
            IsUsePascal = true;
            IsClass = true;
            IsUseProperty = true;
            IsImMutable = false;
            IsMutable = true;
            IsAttributeAdded = false;
            SelectedType = CollectionTypes.FirstOrDefault(x => x == OutputCollectionType.MutableList);

        }
        private void ConfigureGenerator(IJsonClassGeneratorConfig config)
        {
            config.UsePascalCase = IsUsePascal;
            config.AlwaysUseNullableValues = IsUseNullableValues;
            config.CollectionType = SelectedType;
            if (IsClass)
            {
                if (IsImMutable)
                {
                    config.OutputType = OutputTypes.ImmutableClass;
                }
                else
                {
                    config.OutputType = OutputTypes.MutableClass;
                    config.MutableClasses.Members = IsUseProperty ? OutputMembers.AsProperties : OutputMembers.AsPublicFields;
                }
                if (IsAttributeAdded)
                {
                    config.AttributeUsage = JsonPropertyAttributeUsage.Always;
                    config.AttributeLibrary = IsJsonPropertyAttribute ? JsonLibrary.NewtonsoftJson : JsonLibrary.SystemTextJson;
                }
                else
                {
                    config.AttributeUsage = JsonPropertyAttributeUsage.OnlyWhenNecessary;
                }
            }
            else if (IsRecord)
            {
                config.OutputType = OutputTypes.ImmutableRecord;
            }
        }
        //public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        //{
        //    _ = SettingsStorageExtensions.SaveSettingAsync(PageTokens.JsonToCSharpePage, nameof(PageTokens));
        //}
        public ICommand CopyCommand => new DelegateCommand(OnCopyCommandExecute);

        private async Task OnExportCommandExecuteAsync()
        {
            if (!string.IsNullOrEmpty(OutputText))
            {
                await FileHelper.SaveFileAsync(OutputText, FileTypes.CS, "root.cs");
            }
        }

        private void OnCopyCommandExecute()
        {
            if (!string.IsNullOrEmpty(OutputText))
            {
                FileHelper.CopyText(OutputText);
            }
        }
        public ICommand ExportCommand => new AsyncCommand(OnExportCommandExecuteAsync);

        public ICommand ClearCommand => new DelegateCommand(OnClearCommandExecute);

        private void OnClearCommandExecute()
        {
            Setup();
            InputText = string.Empty;
            OutputText = string.Empty;
        }

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
            if (obj.Key == VirtualKey.Escape)
            {
                OnClearCommandExecute();
            }
        }

        public JsonToCSharpeViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            Setup();
        }
        public OutputCollectionType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }
        public bool IsUsePascal
        {
            get => _isUsePascal;
            set
            {
                _isUsePascal = value;
                OnPropertyChanged();
            }
        }
        public bool IsUseProperty
        {
            get => _isUseProperty;
            set
            {
                _isUseProperty = value;
                OnPropertyChanged();
            }
        }
        public bool IsUseNullableValues
        {
            get => _isUseNullableValues;
            set
            {
                _isUseNullableValues = value;
                OnPropertyChanged();
            }
        }
        public bool IsClass
        {
            get => _isClass;
            set
            {
                _isClass = value;
                OnPropertyChanged();
            }
        }
        public bool IsInpc
        {
            get => _isInpc;
            set
            {
                _isInpc = value;
                OnPropertyChanged();
            }
        }
        public bool IsRecord
        {
            get => _isRecord;
            set
            {
                _isRecord = value;
                OnPropertyChanged();
            }
        }
        public bool IsPrism
        {
            get => _isPrism;
            set
            {
                _isPrism = value;
                OnPropertyChanged();
            }
        }
        public bool IsImMutable
        {
            get => _isImMutable;
            set
            {
                _isImMutable = value;
                OnPropertyChanged();
            }
        }
        public bool IsMutable
        {
            get => _isMutable;
            set
            {
                _isMutable = value;
                OnPropertyChanged();
            }
        }
        public bool IsAttributeAdded
        {
            get => _isAttributeAdded;
            set
            {
                _isAttributeAdded = value;
                OnPropertyChanged();
            }
        }
        public bool IsJsonPropertyAttribute
        {
            get => _isJsonPropertyAttribute;
            set
            {
                _isJsonPropertyAttribute = value;
                OnPropertyChanged();
            }
        }

    }
}

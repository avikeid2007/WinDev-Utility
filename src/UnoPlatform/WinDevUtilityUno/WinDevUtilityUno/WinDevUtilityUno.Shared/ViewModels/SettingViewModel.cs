using BasicMvvm;
using BasicMvvm.Commands;

using System.Windows.Input;

using WinDevUtilityUno.Helpers;

using Windows.ApplicationModel;

using Windows.UI.Xaml;

namespace WinDevUtilityUno.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        public SettingViewModel()
        {
            Initialize();

        }
        private ElementTheme _elementTheme = ThemeSelectorHelper.Theme;
        public ElementTheme ElementTheme
        {
            get => _elementTheme;
            set
            {
                _elementTheme = value;
                OnPropertyChanged();
            }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get => _versionDescription;
            set
            {
                _versionDescription = value;
                OnPropertyChanged();
            }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                return _switchThemeCommand ??= new DelegateCommand<object>(
                        async (param) =>
                        {
                            ElementTheme = (ElementTheme)param;
                            await ThemeSelectorHelper.SetThemeAsync((ElementTheme)param);
                        });
            }
        }
        public void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var appName = "WinDev Utility";
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }



    }
}

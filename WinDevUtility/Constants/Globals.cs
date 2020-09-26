namespace WinDevUtility.Constants
{
    public static class Globals
    {
        #region [Xaml Page]
        public const string GridTextstr = @"<Grid>
<Grid.RowDefinitions>
{0}

</Grid.RowDefinitions>
<Grid.ColumnDefinitions>
{1}

</Grid.ColumnDefinitions>
{2}

</Grid>";
        public const string StackTextstr = @"<StackPanel>
{0}

</StackPanel>";

        public const string StackTextXamarinstr = @"<StackLayout>
{0}

</StackLayout>";
        public const string RowTextstr = @"<RowDefinition Height=""{0}"" />";
        public const string ColumnTextstr = @"<ColumnDefinition Width=""{0}"" />";
        public const string DateControlstr = @"<DatePicker Date=""{{{0} {1}}}"" {2} />";
        public const string DateControlWPFstr = @"<DatePicker SelectedDate=""{{{0} {1}}}"" {2} />";
        public const string ComboControlstr = @"<ComboBox SelectedItem=""{{{0} {1}}}"" {2} />";
        public const string ComboControlXamarinstr = @"<Picker SelectedItem=""{{{0} {1}}}"" {2} />";
        public const string TextBlockControlXamarinstr = @"<Label Text=""{{{0} {1}}}"" {2} />";
        public const string TextBlockControlstr = @"<TextBlock Text=""{{{0} {1}}}"" {2} />";
        public const string TextBoxControlXamarinstr = @"<Entry Text=""{{{0} {1}}}"" {2} />";
        public const string TextBoxControlstr = @"<TextBox Text=""{{{0} {1}}}"" {2} />";
        public const string CheckBoxControlstr = @"<CheckBox IsChecked=""{{{0} {1}}}"" {2} />";
        public const string ListViewControlstr = @"<ListView ItemsSource=""{{{0} {1}}}"" {2} />";
        #endregion

        #region [Command Page]
        public const string icommandStr = "ICommand";
        public const string relayCommandStr = "RelayCommand";
        public const string delegateCommandStr = "DelegateCommand";
        public const string AsyncCommandStr = "AsyncCommand";
        public const string publicStr = "public";
        public const string getSetstr = "{ get; set; }";
        public const string executeMethodstr = "On{0}Executed";
        public const string canExecuteMethodstr = "Can{0}Executed";
        public const string executeMethodDefination = @"private void {0}({1})
{{ 
    throw new NotImplementedException(); 
}}";
        public const string canExecuteMethodDefination = @"private bool {0}({1})
{{ 
    throw new NotImplementedException(); 
}}";
        public const string executeAsyncMethodDefination = @"private Task {0}({1})
{{ 
    throw new NotImplementedException();
}}";
        #endregion
        public const string PocoPlaceHolder = @"Enter properties and variables with or without access modifier i.e.
string Username
bool _isNull
List<string> _productName;
int userId;
string   haveExtraSpace;
Books bookClassObject
private Employee privateObjectDeceleration
public readonly Post WithReadOnlyKeyword
public string Poco {get; set;}
public virtual ICollection<Blogs> WithVirtualKeyword";
    }
}

namespace WinDevUtility.Constants
{
    public static class Globals
    {
        public const string GridTextstr = @"<Grid>
<Grid.RowDefinitions>
{0}</Grid.RowDefinitions>
<Grid.ColumnDefinitions>
{1}</Grid.ColumnDefinitions>
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
    }
}

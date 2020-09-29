using System;
using System.Linq;
using WinDevUtility.Constants;
using Windows.UI.Xaml.Data;

namespace WinDevUtility.Converters
{
    public class FavouriteConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Globals.FavList?.Count > 0 && value is string id)
            {
                return Globals.FavList.Any(x => x.InternalID == id);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTracker
{
    internal class UserStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var userStatus = (UserStatusEnum)value;
            var parameterString = parameter as string;

            if (parameterString == "MarkCurrentlyViewing")
            {
                return userStatus == UserStatusEnum.WATCHLIST;
            }
            else if (parameterString == "MarkCompleted")
            {
                return userStatus == UserStatusEnum.WATCHLIST || userStatus == UserStatusEnum.CURRENTLY_VIEWING;
            }
            else if (parameterString == "RemoveFromLibrary")
            {
                return true; // Always show the "Remove from Library" button
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
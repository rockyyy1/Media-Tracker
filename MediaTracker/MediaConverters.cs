﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MediaTracker
{
    public class MediaConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AvailabilityStatusEnum availability)
            {
                if (targetType == typeof(Color)) // Color converter
                {
                    return availability == AvailabilityStatusEnum.COMING_SOON ? Colors.Red : Colors.Green;
                }
                else // Text converter
                {
                    return availability.ToString().Replace("_", " ");
                }
            }
            else if (value is UserStatusEnum status)
            {
                switch (status)
                {
                    case UserStatusEnum.WATCHLIST:
                        return "On my Watchlist";
                    case UserStatusEnum.CURRENTLY_VIEWING:
                        return "Currently watching";
                    case UserStatusEnum.COMPLETED:
                        return "Completed";
                    default:
                        return "";
                }
            }
            else if (value is string plot)
            {
                string cleanedPlot = Regex.Replace(plot, "<.*?>", string.Empty);
                return cleanedPlot;
            }
            else if (value is int count)
            {
                return count == 0;
            }
            else if (value is string[] array)
            {
                return string.Join(", ", array);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

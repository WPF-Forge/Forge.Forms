﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Forge.Forms.Utils.ValueConverters
{
    public class AsBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
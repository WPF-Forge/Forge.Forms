﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Forge.Forms.DynamicExpressions.ValueConverters
{
    public class ToLowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as string)?.ToLower(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

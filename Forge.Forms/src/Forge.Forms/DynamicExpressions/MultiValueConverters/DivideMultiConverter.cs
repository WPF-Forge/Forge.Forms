using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.DynamicExpressions.MultiValueConverters
{
    public class DivideMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] != null &&
                values[1] != null)
            {
                if (values[0] is decimal dc1)
                {
                    if (values[1] is decimal dc2)
                    {
                        return dc1 / dc2;
                    }
                    else if (values[1] is double d2)
                    {
                        return dc1 / (decimal)d2;
                    }
                    else if (values[1] is float f2)
                    {
                        return dc1 / (decimal)f2;
                    }
                    else if (values[1] is long l2)
                    {
                        return dc1 / l2;
                    }
                    else if (values[1] is int i2)
                    {
                        return dc1 / i2;
                    }
                    else if (values[1] is short s2)
                    {
                        return dc1 / s2;
                    }
                }
                else if (values[0] is double d1)
                {
                    if (values[1] is decimal dc2)
                    {
                        return d1 / (double)dc2;
                    }
                    else if (values[1] is double d2)
                    {
                        return d1 / d2;
                    }
                    else if (values[1] is float f2)
                    {
                        return d1 / f2;
                    }
                    else if (values[1] is long l2)
                    {
                        return d1 / l2;
                    }
                    else if (values[1] is int i2)
                    {
                        return d1 / i2;
                    }
                    else if (values[1] is short s2)
                    {
                        return d1 / s2;
                    }
                }
                else if (values[0] is float f1)
                {
                    if (values[1] is decimal dc2)
                    {
                        return f1 / (double)dc2;
                    }
                    else if (values[1] is double d2)
                    {
                        return f1 / d2;
                    }
                    else if (values[1] is float f2)
                    {
                        return f1 / f2;
                    }
                    else if (values[1] is long l2)
                    {
                        return f1 / l2;
                    }
                    else if (values[1] is int i2)
                    {
                        return f1 / i2;
                    }
                    else if (values[1] is short s2)
                    {
                        return f1 / s2;
                    }
                }
                else if (values[0] is long l1)
                {
                    if (values[1] is decimal dc2)
                    {
                        return l1 / (double)dc2;
                    }
                    else if (values[1] is double d2)
                    {
                        return l1 / d2;
                    }
                    else if (values[1] is float f2)
                    {
                        return l1 / f2;
                    }
                    else if (values[1] is long l2)
                    {
                        return l1 / l2;
                    }
                    else if (values[1] is int i2)
                    {
                        return l1 / i2;
                    }
                    else if (values[1] is short s2)
                    {
                        return l1 / s2;
                    }
                }
                else if (values[0] is int i1)
                {
                    if (values[1] is decimal dc2)
                    {
                        return i1 / (double)dc2;
                    }
                    else if (values[1] is double d2)
                    {
                        return i1 / d2;
                    }
                    else if (values[1] is float f2)
                    {
                        return i1 / f2;
                    }
                    else if (values[1] is long l2)
                    {
                        return i1 / l2;
                    }
                    else if (values[1] is int i2)
                    {
                        return i1 / i2;
                    }
                    else if (values[1] is short s2)
                    {
                        return i1 / s2;
                    }
                }
                else if (values[0] is short s1)
                {
                    if (values[1] is decimal dc2)
                    {
                        return s1 / (double)dc2;
                    }
                    else if (values[1] is double d2)
                    {
                        return s1 / d2;
                    }
                    else if (values[1] is float f2)
                    {
                        return s1 / f2;
                    }
                    else if (values[1] is long l2)
                    {
                        return s1 / l2;
                    }
                    else if (values[1] is int i2)
                    {
                        return s1 / i2;
                    }
                    else if (values[1] is short s2)
                    {
                        return s1 / s2;
                    }
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
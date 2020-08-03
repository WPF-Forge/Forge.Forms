using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Forge.Forms.Demo.Converters
{
    public class DivideMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values.Length == 2 &&
                    values[0] != null &&
                    values[1] != null)
                {
                    switch (values[0])
                    {
                        case decimal dc1:
                            switch (values[1])
                            {
                                case decimal dc2: return dc1 / dc2;
                                case double d2: return dc1 / (decimal)d2;
                                case float f2: return dc1 / (decimal)f2;
                                case long l2: return dc1 / l2;
                                case int i2: return dc1 / i2;
                                case short s2: return dc1 / s2;
                            }
                            break;
                        case double d1:
                            switch (values[1])
                            {
                                case decimal dc2: return (decimal)d1 / dc2;
                                case double d2: return d1 / d2;
                                case float f2: return d1 / f2;
                                case long l2: return d1 / l2;
                                case int i2: return d1 / i2;
                                case short s2: return d1 / s2;
                            }
                            break;
                        case float f1:
                            switch (values[1])
                            {
                                case decimal dc2: return (decimal)f1 / dc2;
                                case double d2: return f1 / d2;
                                case float f2: return f1 / f2;
                                case long l2: return f1 / l2;
                                case int i2: return f1 / i2;
                                case short s2: return f1 / s2;
                            }
                            break;
                        case long l1:
                            switch (values[1])
                            {
                                case decimal dc2: return l1 / dc2;
                                case double d2: return l1 / d2;
                                case float f2: return l1 / f2;
                                case long l2: return l1 / l2;
                                case int i2: return l1 / i2;
                                case short s2: return l1 / s2;
                            }
                            break;
                        case int i1:
                            switch (values[1])
                            {
                                case decimal dc2: return i1 / dc2;
                                case double d2: return i1 / d2;
                                case float f2: return i1 / f2;
                                case long l2: return i1 / l2;
                                case int i2: return i1 / i2;
                                case short s2: return i1 / s2;
                            }
                            break;
                        case short s1:
                            switch (values[1])
                            {
                                case decimal dc2: return s1 / dc2;
                                case double d2: return s1 / d2;
                                case float f2: return s1 / f2;
                                case long l2: return s1 / l2;
                                case int i2: return s1 / i2;
                                case short s2: return s1 / s2;
                            }
                            break;
                    }
                }
            }
            catch (DivideByZeroException)
            {
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
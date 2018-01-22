using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows.Data;

namespace Forge.Forms.DynamicExpressions.ValueConverters
{
    public class LengthValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string s:
                    return s.Length;
                case IEnumerable<object> e:
                    return e.Count();
                case IEnumerable e:
                    return Count(e);
                case SecureString s:
                    return s.Length;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static int Count(IEnumerable source)
        {
            if (source is ICollection collection)
            {
                return collection.Count;
            }

            var count = 0;
            var e = source.GetEnumerator();
            try
            {
                while (e.MoveNext())
                {
                    count++;
                }

                return count;
            }
            finally
            {
                (e as IDisposable)?.Dispose();
            }
        }
    }
}

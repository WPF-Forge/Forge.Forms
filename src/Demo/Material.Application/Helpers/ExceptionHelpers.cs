using System;
using System.IO;

namespace Material.Application.Helpers
{
    public class ExceptionHelpers
    {
        public static void LogException(string prefix, Exception exception, string path)
        {
            if (exception == null)
            {
                return;
            }

            try
            {
                using (var streamWriter = new StreamWriter(path))
                {
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        streamWriter.WriteLine(prefix);
                        streamWriter.WriteLine();
                    }

                    streamWriter.WriteLine("-Message-\r\n{0}", exception.Message);
                    streamWriter.WriteLine("\r\n-Source-\r\n{0}", exception.Source);
                    streamWriter.WriteLine("\r\n-TargetSite-\r\n{0}", exception.TargetSite);
                    streamWriter.WriteLine("\r\n-StackTrace-\r\n{0}", exception.StackTrace);
                    exception = exception.InnerException;
                    while (exception != null)
                    {
                        streamWriter.WriteLine("-Inner Exception-\r\n");
                        streamWriter.WriteLine("-Message-\r\n{0}", exception.Message);
                        streamWriter.WriteLine("\r\n-Source-\r\n{0}", exception.Source);
                        streamWriter.WriteLine("\r\n-TargetSite-\r\n{0}", exception.TargetSite);
                        streamWriter.WriteLine("\r\n-StackTrace-\r\n{0}", exception.StackTrace);
                        exception = exception.InnerException;
                    }
                }
            }
            catch
            {
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Ninject.Parameters;

namespace Material.Application.Helpers
{
    internal static class IocHelpers
    {
        public static IParameter MapParameter(KeyValuePair<string, object> parameter)
        {
            var key = parameter.Key;
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Null or empty key provided as parameter name.");
            }

            if (char.IsUpper(key[0]))
            {
                return new PropertyValue(key, parameter.Value);
            }

            return new ConstructorArgument(key, parameter.Value);
        }
    }
}

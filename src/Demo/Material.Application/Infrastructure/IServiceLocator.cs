using System;
using System.Collections.Generic;

namespace Material.Application.Infrastructure
{
    public interface IServiceLocator
    {
        T Get<T>(IDictionary<string, object> parameters);

        object Get(Type type, IDictionary<string, object> parameters);
    }

    public static class ServiceLocatorExtensions
    {
        public static T Get<T>(this IServiceLocator serviceLocator) => serviceLocator.Get<T>(null);

        public static object Get(this IServiceLocator serviceLocator, Type type) => serviceLocator.Get(type, null);

        public static T Get<T>(this IServiceLocator serviceLocator,
            Action<IDictionary<string, object>> parametersInitializer)
        {
            var parameters = new Dictionary<string, object>();
            parametersInitializer?.Invoke(parameters);
            return serviceLocator.Get<T>(parameters);
        }

        public static T Get<T>(this IServiceLocator serviceLocator, string parameterName, object parameterValue)
            => serviceLocator.Get<T>(new Dictionary<string, object>
            {
                [parameterName] = parameterValue
            });

        public static T Get<T>(this IServiceLocator serviceLocator, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value) => serviceLocator.Get<T>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value
            });

        public static T Get<T>(this IServiceLocator serviceLocator, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value)
            => serviceLocator.Get<T>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value
            });

        public static T Get<T>(this IServiceLocator serviceLocator, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value,
            string parameter4Name,
            object parameter4Value) => serviceLocator.Get<T>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value,
                [parameter4Name] = parameter4Value
            });

        public static T Get<T>(this IServiceLocator serviceLocator, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value,
            string parameter4Name,
            object parameter4Value, string parameter5Name, object parameter5Value)
            => serviceLocator.Get<T>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value,
                [parameter4Name] = parameter4Value,
                [parameter5Name] = parameter5Value
            });

        public static T Get<T>(this IServiceLocator serviceLocator, string parameter1Name, object parameter1Value,
            string parameter2Name, object parameter2Value, string parameter3Name, object parameter3Value,
            string parameter4Name, object parameter4Value, string parameter5Name, object parameter5Value,
            string parameter6Name,
            object parameter6Value) => serviceLocator.Get<T>(new Dictionary<string, object>
            {
                [parameter1Name] = parameter1Value,
                [parameter2Name] = parameter2Value,
                [parameter3Name] = parameter3Value,
                [parameter4Name] = parameter4Value,
                [parameter5Name] = parameter5Value,
                [parameter6Name] = parameter6Value
            });
    }
}

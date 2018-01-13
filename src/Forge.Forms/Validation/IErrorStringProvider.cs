using Forge.Forms.Utils;

namespace Forge.Forms.Validation
{
    public interface IErrorStringProvider
    {
        string GetErrorMessage(object value);
    }

    public class ValueErrorStringProvider : IErrorStringProvider
    {
        private readonly StringProxy messageProxy;

        private readonly BindingProxy valueProxy;

        public ValueErrorStringProvider(StringProxy messageProxy, BindingProxy valueProxy)
        {
            this.messageProxy = messageProxy;
            this.valueProxy = valueProxy;
        }

        public string GetErrorMessage(object value)
        {
            valueProxy.SetCurrentValue(BindingProxy.ValueProperty, value);
            return messageProxy.Value;
        }
    }

    public class ErrorStringProvider : IErrorStringProvider
    {
        private readonly StringProxy messageProxy;

        public ErrorStringProvider(StringProxy messageProxy)
        {
            this.messageProxy = messageProxy;
        }

        public string GetErrorMessage(object value)
        {
            return messageProxy.Value;
        }
    }

    public class PlainErrorStringProvider : IErrorStringProvider
    {
        public PlainErrorStringProvider(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public string GetErrorMessage(object value)
        {
            return ErrorMessage;
        }
    }
}

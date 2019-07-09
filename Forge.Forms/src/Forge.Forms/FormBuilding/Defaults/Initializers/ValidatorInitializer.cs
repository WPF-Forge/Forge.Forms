using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.Validation;

namespace Forge.Forms.FormBuilding.Defaults.Initializers
{
    internal class ValidatorInitializer : IFieldInitializer
    {
        public void Initialize(FormElement element, IFormProperty property, Func<string, object> deserializer)
        {
            if (!(element is DataFormField dataField))
            {
                return;
            }

            var attributes = property.GetCustomAttributes<ValueAttribute>().ToArray();
            if (attributes.Length == 0)
            {
                return;
            }

            var modelType = property.DeclaringType;
            foreach (var attr in attributes)
            {
                dataField.Validators.Add(CreateValidator(modelType, dataField.Key, attr, deserializer));
            }
        }

        private static IValidatorProvider CreateValidator(Type modelType, string propertyKey, ValueAttribute attribute,
            Func<string, object> deserializer)
        {
            Func<IResourceContext, IProxy> argumentProvider;
            var argument = attribute.Argument;
            if (argument is string expression)
            {
                var boundExpression = BoundExpression.Parse(expression);
                if (boundExpression.IsPlainString)
                {
                    var literal = new PlainObject(deserializer != null
                        ? deserializer(boundExpression.StringFormat)
                        : boundExpression.StringFormat);

                    argumentProvider = context => literal;
                }
                else
                {
                    var getString = boundExpression.StringFormat != null;
                    var action = attribute.ArgumentUpdatedAction;
                    var notify = action == ValidationAction.ClearErrors || action == ValidationAction.ValidateField;
                    argumentProvider = context =>
                    {
                        var value = getString
                            ? (IProxy)boundExpression.GetStringValue(context)
                            : boundExpression.GetValue(context);

                        if (notify)
                        {
                            value.ValueChanged = () =>
                            {
                                object model;
                                try
                                {
                                    model = context.GetModelInstance();
                                }
                                catch
                                {
                                    // Something went wrong so it's best to 
                                    // disable the feature entirely.
                                    value.ValueChanged = null;
                                    return;
                                }

                                //delete old logic (if model is ExpandoObject then do nothing)
                                if (model == null || model.GetType() != modelType)
                                {
                                    // Self dispose when form indicates model change.
                                    value.ValueChanged = null;
                                    return;
                                }

                                if (action == ValidationAction.ValidateField)
                                {
                                    ModelState.Validate(model, propertyKey);
                                }
                                else if (action == ValidationAction.ClearErrors)
                                {
                                    ModelState.ClearValidationErrors(model, propertyKey);
                                }
                            };
                        }

                        return value;
                    };
                }
            }
            else
            {
                var literal = new PlainObject(argument);
                argumentProvider = context => literal;
            }

            BindingProxy ValueProvider(IResourceContext context)
            {
                var key = new BindingProxyKey(propertyKey);
                if (context.TryFindResource(key) is BindingProxy proxy)
                {
                    return proxy;
                }

                proxy = new BindingProxy();
                context.AddResource(key, proxy);
                return proxy;
            }

            Func<IResourceContext, IBoolProxy> isEnforcedProvider;
            switch (attribute.When)
            {
                case null:
                    isEnforcedProvider = context => new PlainBool(true);
                    break;
                case string expr:
                    var boundExpression = BoundExpression.Parse(expr);
                    if (!boundExpression.IsSingleResource)
                    {
                        throw new ArgumentException(
                            "The provided value must be a bound resource or a literal bool value.", nameof(attribute));
                    }

                    var onActivation = attribute.OnActivation;
                    var onDeactivation = attribute.OnDeactivation;

                    var notifyActivation = onActivation == ValidationAction.ClearErrors || onActivation == ValidationAction.ValidateField;
                    var notifyDeactivation = onDeactivation == ValidationAction.ClearErrors || onDeactivation == ValidationAction.ValidateField;
                    isEnforcedProvider = context =>
                    {
                        var value = boundExpression.GetBoolValue(context);
                        if (notifyActivation || notifyDeactivation)
                        {
                            value.ValueChanged = () =>
                            {
                                object model;
                                try
                                {
                                    model = context.GetModelInstance();
                                }
                                catch
                                {
                                    // Something went wrong so it's best to 
                                    // disable the feature entirely.
                                    value.ValueChanged = null;
                                    return;
                                }

                                //delete old logic (if model is ExpandoObject then do nothing)
                                if (model == null || model.GetType() != modelType)
                                {
                                    // Self dispose when form indicates model change.
                                    value.ValueChanged = null;
                                    return;
                                }

                                if (value.Value)
                                {
                                    // Activated.
                                    if (onActivation == ValidationAction.ValidateField)
                                    {
                                        ModelState.Validate(model, propertyKey);
                                    }
                                    else if (onActivation == ValidationAction.ClearErrors)
                                    {
                                        ModelState.ClearValidationErrors(model, propertyKey);
                                    }
                                }
                                else
                                {
                                    // Deactivated.
                                    if (onDeactivation == ValidationAction.ValidateField)
                                    {
                                        ModelState.Validate(model, propertyKey);
                                    }
                                    else if (onDeactivation == ValidationAction.ClearErrors)
                                    {
                                        ModelState.ClearValidationErrors(model, propertyKey);
                                    }
                                }
                            };
                        }

                        return value;
                    };
                    break;
                case bool b:
                    isEnforcedProvider = context => new PlainBool(b);
                    break;
                default:
                    throw new ArgumentException(
                        "The provided value must be a bound resource or a literal bool value.", nameof(attribute));
            }

            Func<IResourceContext, IErrorStringProvider> errorProvider;
            var message = attribute.Message;
            if (message == null)
            {
                switch (attribute.Condition)
                {
                    case Must.BeGreaterThan:
                        message = "Value must be greater than {Argument}.";
                        break;
                    case Must.BeGreaterThanOrEqualTo:
                        message = "Value must be greater than or equal to {Argument}.";
                        break;
                    case Must.BeLessThan:
                        message = "Value must be less than {Argument}.";
                        break;
                    case Must.BeLessThanOrEqualTo:
                        message = "Value must be less than or equal to {Argument}.";
                        break;
                    case Must.BeEmpty:
                        message = "@Field must be empty.";
                        break;
                    case Must.NotBeEmpty:
                        message = "@Field cannot be empty.";
                        break;
                    default:
                        message = "@Invalid value.";
                        break;
                }
            }

            {
                var func = new Func<IResourceContext, IProxy>(ValueProvider);
                var boundExpression = BoundExpression.Parse(message, new Dictionary<string, object>
                {
                    ["Value"] = func,
                    ["Argument"] = argumentProvider
                });

                if (boundExpression.IsPlainString)
                {
                    var errorMessage = boundExpression.StringFormat;
                    errorProvider = context => new PlainErrorStringProvider(errorMessage);
                }
                else
                {
                    if (boundExpression.Resources.Any(
                        res => res is DeferredProxyResource resource && resource.ProxyProvider == func))
                    {
                        errorProvider =
                            context => new ValueErrorStringProvider(boundExpression.GetStringValue(context),
                                ValueProvider(context));
                    }
                    else
                    {
                        errorProvider =
                            context => new ErrorStringProvider(boundExpression.GetStringValue(context));
                    }
                }
            }

            var converterName = attribute.Converter;

            IValueConverter GetConverter(IResourceContext context)
            {
                IValueConverter converter = null;
                if (converterName != null)
                {
                    converter = Resource.GetValueConverter(context, converterName);
                }

                return converter;
            }
            //NullValueValidateAction nullValueValidateAction = NullValueValidateAction.Default
            var strictValidation = attribute.StrictValidation;
            var validateOnTargetUpdated = attribute.ValidatesOnTargetUpdated;
            var nullValueValidation = attribute.NullValueValidation;
            switch (attribute.Condition)
            {
                case Must.BeEqualTo:
                    return new ValidatorProvider((context, pipe) => new EqualsValidator(pipe, argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.NotBeEqualTo:
                    return new ValidatorProvider((context, pipe) => new NotEqualsValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.BeGreaterThan:
                    return new ValidatorProvider((context, pipe) => new GreaterThanValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.BeGreaterThanOrEqualTo:
                    return new ValidatorProvider((context, pipe) => new GreaterThanEqualValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.BeLessThan:
                    return new ValidatorProvider((context, pipe) => new LessThanValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.BeLessThanOrEqualTo:
                    return new ValidatorProvider((context, pipe) => new LessThanEqualValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.BeEmpty:
                    return new ValidatorProvider((context, pipe) => new EmptyValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                case Must.NotBeEmpty:
                    return new ValidatorProvider((context, pipe) => new NotEmptyValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                case Must.BeTrue:
                    return new ValidatorProvider((context, pipe) => new TrueValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                case Must.BeFalse:
                    return new ValidatorProvider((context, pipe) => new FalseValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                case Must.BeNull:
                    return new ValidatorProvider((context, pipe) => new NullValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                case Must.NotBeNull:
                    return new ValidatorProvider((context, pipe) => new NotNullValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                case Must.ExistIn:
                    return new ValidatorProvider((context, pipe) => new ExistsInValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.NotExistIn:
                    return new ValidatorProvider((context, pipe) => new NotExistsInValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.MatchPattern:
                    return new ValidatorProvider((context, pipe) => new MatchPatternValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.NotMatchPattern:
                    return new ValidatorProvider((context, pipe) => new NotMatchPatternValidator(pipe,
                        argumentProvider(context),
                        errorProvider(context), isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated, nullValueValidation));
                case Must.SatisfyContextMethod:
                    var methodName = GetMethodName(attribute.Argument, propertyKey);
                    var propertyName = propertyKey;
                    return new ValidatorProvider(
                        (context, pipe) => new MethodInvocationValidator(pipe,
                            GetContextMethodValidator(propertyName, methodName, context),
                            errorProvider(context), isEnforcedProvider(context), GetConverter(context),
                            strictValidation,
                            validateOnTargetUpdated));
                case Must.SatisfyMethod:
                    var type = modelType;
                    methodName = GetMethodName(attribute.Argument, propertyKey);
                    propertyName = propertyKey;
                    return new ValidatorProvider(
                        (context, pipe) => new MethodInvocationValidator(pipe,
                            GetModelMethodValidator(type, propertyName, methodName, context),
                            errorProvider(context), isEnforcedProvider(context), GetConverter(context),
                            strictValidation,
                            validateOnTargetUpdated));
                case Must.BeInvalid:
                    return new ValidatorProvider((context, pipe) => new EnforcedValidator(pipe, errorProvider(context),
                        isEnforcedProvider(context), GetConverter(context), strictValidation,
                        validateOnTargetUpdated));
                default:
                    throw new ArgumentException($"Invalid validator condition for property {propertyKey}.",
                        nameof(attribute));
            }
        }

        private static string GetMethodName(object argument, string propertyKey)
        {
            if (argument is string methodName && !string.IsNullOrWhiteSpace(methodName))
            {
                return methodName;
            }

            throw new InvalidOperationException(
                $"Validator for property {propertyKey} does not specify a valid method name. Value must be a nonempty string.");
        }

        // Called on binding -> not performance critical.
        private static Func<object, CultureInfo, bool, bool> GetModelMethodValidator(Type modelType,
            string propertyName, string methodName, IResourceContext context)
        {
            var method = GetMethod(modelType, methodName);
            if (method == null)
            {
                throw new InvalidOperationException(
                    $"Type hierarchy of {modelType.FullName} does not include a static method named {methodName}.");
            }

            // Called on validation -> performance critical.
            bool Validate(object value, CultureInfo cultureInfo, bool strictValidation)
            {
                return method(new ValidationContext(
                    context.GetModelInstance(),
                    context.GetContextInstance(),
                    propertyName,
                    value,
                    cultureInfo,
                    !strictValidation));
            }

            return Validate;
        }

        // Called on binding = not performance critical.
        private static Func<object, CultureInfo, bool, bool> GetContextMethodValidator(string propertyName,
            string methodName, IResourceContext context)
        {
            Type currentType = null;
            Func<ValidationContext, bool> method = null;

            // Called on validation -> performance critical.
            bool Validate(object value, CultureInfo cultureInfo, bool strictValidation)
            {
                // Context type may change in runtime. Change delegate only when necessary.
                var contextInstance = context.GetContextInstance();
                var contextType = contextInstance?.GetType();
                if (contextType != currentType)
                {
                    method = GetMethod(contextType, methodName);
                    currentType = contextType;
                }

                if (method == null)
                {
                    return true;
                }

                return method(new ValidationContext(
                    context.GetModelInstance(),
                    contextInstance,
                    propertyName,
                    value,
                    cultureInfo,
                    !strictValidation));
            }

            return Validate;
        }

        private static Func<ValidationContext, bool> GetMethod(Type type, string methodName)
        {
            var delegateType = typeof(Func<ValidationContext, bool>);

            bool IsMatch(MethodInfo methodInfo)
            {
                if (methodInfo.Name != methodName)
                {
                    return false;
                }

                if (methodInfo.ReturnType != typeof(bool))
                {
                    return false;
                }

                var parameters = methodInfo.GetParameters();
                if (parameters.Length != 1)
                {
                    return false;
                }

                return parameters[0].ParameterType == typeof(ValidationContext);
            }

            var method = type
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .FirstOrDefault(IsMatch);

            if (method == null)
            {
                return null;
            }

            try
            {
                return (Func<ValidationContext, bool>)Delegate.CreateDelegate(delegateType, method);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// make a simple class to provide binding-enfoce indicator
    /// So we don't need to rely on modeltype to determine whether references should be released. 
    /// this means that the expandoObject type model can also use these feature correctly.
    /// </summary>

    internal class BindingEnforcedProvider
    {
        private ValidationAction onActivation;
        private ValidationAction onDeactivation;
        private BoundExpression boundExpression;
        private string propertyKey;
        public BindingEnforcedProvider(string propertyKey, BoundExpression expr, ValidationAction onActivation, ValidationAction onDeactivation)
        {
            this.propertyKey = propertyKey;
            this.onActivation = onActivation;
            this.onDeactivation = onDeactivation;
            this.boundExpression = expr;
        }

        public IBoolProxy ProvideValue(IResourceContext context)
        {
            var value = boundExpression.GetBoolValue(context);
            var notifyActivation = onActivation == ValidationAction.ClearErrors || onActivation == ValidationAction.ValidateField;
            var notifyDeactivation = onDeactivation == ValidationAction.ClearErrors || onDeactivation == ValidationAction.ValidateField;
            if (notifyActivation || notifyDeactivation)
            {
                value.ValueChanged = () =>
                {
                    object model;
                    try
                    {
                        model = context.GetModelInstance();
                    }
                    catch
                    {
                        // Something went wrong so it's best to 
                        // disable the feature entirely.
                        value.ValueChanged = null;
                        return;
                    }

                    if (value.Value)
                    {
                        // Activated.
                        if (onActivation == ValidationAction.ValidateField)
                        {
                            ModelState.Validate(model, propertyKey);
                        }
                        else if (onActivation == ValidationAction.ClearErrors)
                        {
                            ModelState.ClearValidationErrors(model, propertyKey);
                        }
                    }
                    else
                    {
                        // Deactivated.
                        if (onDeactivation == ValidationAction.ValidateField)
                        {
                            ModelState.Validate(model, propertyKey);
                        }
                        else if (onDeactivation == ValidationAction.ClearErrors)
                        {
                            ModelState.ClearValidationErrors(model, propertyKey);
                        }
                    }
                };
            }
            return value;
        }
    }
}

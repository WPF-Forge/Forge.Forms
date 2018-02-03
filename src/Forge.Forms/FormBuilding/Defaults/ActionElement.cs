using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Forge.Forms.Annotations;
using Forge.Forms.DynamicExpressions;
using MaterialDesignThemes.Wpf;

namespace Forge.Forms.FormBuilding.Defaults
{
    public class ActionElement : ContentElement
    {
        /// <summary>
        /// Global action interceptor chain.
        /// Pipes each interceptor output to the next one from left to right.
        /// </summary>
        public static readonly List<IActionInterceptor> InterceptorChain = new List<IActionInterceptor>();

        public IValueProvider Action { get; set; }

        public IValueProvider ActionParameter { get; set; }

        public IValueProvider IsEnabled { get; set; }

        public IValueProvider IsReset { get; set; }

        public IValueProvider Validates { get; set; }

        public IValueProvider ClosesDialog { get; set; }

        public IValueProvider IsLoading { get; set; }

        public IValueProvider IsPrimary { get; set; }

        public IValueProvider IsDefault { get; set; }

        public IValueProvider IsCancel { get; set; }

        public IValueProvider ActionInterceptor { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(IsLoading), IsLoading ?? LiteralValue.False);
            Resources.Add(nameof(IsPrimary), IsPrimary ?? LiteralValue.False);
            Resources.Add(nameof(IsDefault), IsDefault ?? LiteralValue.False);
            Resources.Add(nameof(IsCancel), IsCancel ?? LiteralValue.False);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new ActionPresenter(context, Resources, formResources)
            {
                Command = new ActionElementCommand(
                    context,
                    Action,
                    ActionParameter,
                    IsEnabled,
                    Validates,
                    ClosesDialog,
                    IsReset,
                    IsDefault,
                    ActionInterceptor),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment =
                    LinePosition == Position.Left ? HorizontalAlignment.Left : HorizontalAlignment.Right
            };
        }
    }

    internal class ActionElementCommand : ICommand
    {
        private readonly IProxy action;
        private readonly IProxy actionParameter;
        private readonly IProxy actionInterceptor;

        private readonly IBoolProxy canExecute;
        private readonly IBoolProxy closesDialog;
        private readonly IResourceContext context;
        private readonly IBoolProxy resets;
        private readonly IBoolProxy validates;
        private readonly IBoolProxy isDefault;

        public ActionElementCommand(IResourceContext context,
            IValueProvider action,
            IValueProvider actionParameter,
            IValueProvider isEnabled,
            IValueProvider validates,
            IValueProvider closesDialog,
            IValueProvider resets,
            IValueProvider isDefault,
            IValueProvider actionInterceptor)
        {
            this.context = context;
            this.action = action?.GetBestMatchingProxy(context) ?? new PlainObject(null);

            switch (isEnabled)
            {
                case LiteralValue v when v.Value is bool b:
                    canExecute = new PlainBool(b);
                    break;
                case null:
                    canExecute = new PlainBool(true);
                    break;
                default:
                    var proxy = isEnabled.GetBoolValue(context);
                    proxy.ValueChanged = () => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    canExecute = proxy;
                    break;
            }

            this.validates = validates != null
                ? (IBoolProxy)validates.GetBoolValue(context)
                : new PlainBool(false);
            this.closesDialog = closesDialog != null
                ? (IBoolProxy)closesDialog.GetBoolValue(context)
                : new PlainBool(true);
            this.resets = resets != null
                ? (IBoolProxy)resets.GetBoolValue(context)
                : new PlainBool(false);
            this.isDefault = isDefault != null
                ? (IBoolProxy)isDefault.GetBoolValue(context)
                : new PlainBool(false);

            this.actionParameter = actionParameter?.GetBestMatchingProxy(context) ?? new PlainObject(null);
            this.actionInterceptor = (IProxy)actionInterceptor?.GetValue(context) ?? new PlainObject(null);
        }

        public void Execute(object parameter)
        {
            var arg = actionParameter.Value;
            var model = context.GetModelInstance();

            if (resets.Value && ModelState.IsModel(model))
            {
                ModelState.Reset(model);
            }
            else if (validates.Value && ModelState.IsModel(model))
            {
                var isValid = ModelState.Validate(model);
                if (!isValid)
                {
                    return;
                }
            }
            else if (isDefault.Value && ModelState.IsModel(model))
            {
                foreach (var binding in context.GetBindings())
                {
                    binding.UpdateSource();
                }
            }

            var modelContext = context.GetContextInstance();
            IActionContext actionContext = new ActionContext(model, 
                modelContext,
                action.Value, 
                actionParameter.Value,
                context);
            foreach (var globalInterceptor in ActionElement.InterceptorChain)
            {
                actionContext = globalInterceptor.InterceptAction(actionContext);
                if (actionContext == null)
                {
                    // A null indicates cancellation.
                    return;
                }
            }

            if (actionInterceptor.Value is IActionInterceptor localInterceptor)
            {
                // Local interceptor.
                actionContext = localInterceptor.InterceptAction(actionContext);
            }

            if (actionContext == null)
            {
                // A null indicates cancellation.
                return;
            }

            switch (action.Value)
            {
                case string _:
                    if (model is IActionHandler modelHandler)
                    {
                        modelHandler.HandleAction(actionContext);
                    }

                    if (modelContext is IActionHandler contextHandler)
                    {
                        contextHandler.HandleAction(actionContext);
                    }

                    context.OnAction(actionContext);
                    break;
                case ICommand command:
                    command.Execute(arg);
                    break;
            }

            if (closesDialog.Value && context is IFrameworkResourceContext fwContext)
            {
                var frameworkElement = fwContext.GetOwningElement();
                if (frameworkElement != null && frameworkElement.CheckAccess())
                {
                    DialogHost.CloseDialogCommand.Execute(arg, frameworkElement);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            var flag = canExecute.Value;
            if (!flag)
            {
                return false;
            }

            if (action.Value is ICommand command)
            {
                return command.CanExecute(actionParameter.Value);
            }

            return true;
        }

        public event EventHandler CanExecuteChanged;
    }

    public class ActionPresenter : BindingProvider
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand),
            typeof(ActionPresenter),
            new PropertyMetadata(null));

        static ActionPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActionPresenter),
                new FrameworkPropertyMetadata(typeof(ActionPresenter)));
        }

        public ActionPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
            IDictionary<string, IValueProvider> formResources)
            : base(context, fieldResources, formResources, true)
        {
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}

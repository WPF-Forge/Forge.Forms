using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Forge.Forms.Controls;
using Forge.Forms.Interfaces;
using Forge.Forms.Mappers;
using Forge.Forms.Utils;
using MaterialDesignThemes.Wpf;
using Proxier.Mappers;

namespace Forge.Forms.Components.Fields.Defaults
{
    public class ActionElement : ContentElement
    {
        public IValueProvider ActionName { get; set; }

        public IValueProvider ActionParameter { get; set; }

        public IValueProvider IsEnabled { get; set; }

        public IValueProvider IsReset { get; set; }

        public IValueProvider Validates { get; set; }

        public IValueProvider ClosesDialog { get; set; }

        public IValueProvider IsLoading { get; set; }

        public IValueProvider IsDefault { get; set; }

        public IValueProvider IsCancel { get; set; }

        protected internal override void Freeze()
        {
            base.Freeze();
            Resources.Add(nameof(IsLoading), IsLoading ?? LiteralValue.False);
            Resources.Add(nameof(IsDefault), IsDefault ?? LiteralValue.False);
            Resources.Add(nameof(IsCancel), IsCancel ?? LiteralValue.False);
        }

        protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
            IDictionary<string, IValueProvider> formResources)
        {
            return new ActionPresenter(context, Resources, formResources)
            {
                Command = new ActionElementCommand(context, ActionName, ActionParameter, IsEnabled, Validates,
                    ClosesDialog, IsReset),
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
        private readonly IResourceContext context;

        private readonly IBoolProxy canExecute;
        private readonly IBoolProxy closesDialog;
        private readonly IBoolProxy resets;
        private readonly IBoolProxy validates;

        public ActionElementCommand(IResourceContext context, IValueProvider action, IValueProvider actionParameter,
            IValueProvider isEnabled, IValueProvider validates, IValueProvider closesDialog, IValueProvider isReset)
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

            this.validates = validates != null ? (IBoolProxy) validates.GetBoolValue(context) : new PlainBool(false);
            this.closesDialog = closesDialog != null
                ? (IBoolProxy) closesDialog.GetBoolValue(context)
                : new PlainBool(true);
            resets = isReset != null ? (IBoolProxy) isReset.GetBoolValue(context) : new PlainBool(false);
            this.actionParameter = actionParameter?.GetBestMatchingProxy(context) ?? new PlainObject(null);
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
            else
            {
                if (ModelState.IsModel(model))
                    foreach (var binding in context.GetBindings())
                    {
                        binding.UpdateSource();
                    }
            }

            if (closesDialog.Value && context is IFrameworkResourceContext fwContext)
            {
                var frameworkElement = fwContext.GetOwningElement();
                if (frameworkElement != null && frameworkElement.CheckAccess())
                {
                    DialogHost.CloseDialogCommand.Execute(arg, frameworkElement);
                }
            }

            switch (action.Value)
            {
                case string actionName:
                    if (model is IActionHandler modelHandler)
                    {
                        modelHandler.HandleAction(
                            model.GetInjectedObject()
                                ?.CopyTo(model.GetType().FindOverridableType()?.BaseType ?? model.GetType()) ?? model,
                            actionName, arg);
                    }

                    if (context.GetContextInstance() is IActionHandler contextHandler)
                    {
                        contextHandler.HandleAction(
                            model.GetInjectedObject()
                                ?.CopyTo(model.GetType().FindOverridableType()?.BaseType ?? model.GetType()) ?? model,
                            actionName, arg);
                    }

                    model.GetType().FindOverridableType<MaterialMapper>()?.HandleAction(model, actionName, arg);
                    context.OnAction(model, actionName, arg);
                    break;
                case ICommand command:
                    command.Execute(arg);
                    break;
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
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
    }
}
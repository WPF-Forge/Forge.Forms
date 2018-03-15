using System;
using System.Runtime.CompilerServices;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;
using Forge.Forms.FormBuilding.Defaults;

namespace Forge.Forms.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class ActionAttribute : FormContentAttribute
    {
        public ActionAttribute(string name, string content, [CallerLineNumber] int position = 0)
            : base(position)
        {
            ActionName = name;
            Content = content;
            // Actions are grouped by default.
            ShareLine = true;
            // Actions are inserted after elements by default.
            InsertAfter = true;
            // Actions are displayed to the right by default.
            LinePosition = Annotations.Position.Right;
        }

        /// <summary>
        /// Determines whether the enter key will invoke the action.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsDefault { get; set; }

        /// <summary>
        /// Determines whether the escape key will invoke the action.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsCancel { get; set; }

        /// <summary>
        /// Action identifier that is passed to handlers.
        /// </summary>
        public string ActionName { get; }

        /// <summary>
        /// Action parameter. Accepts a dynamic expression.
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Displayed content. Accepts a dynamic expression.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Determines whether this action can be performed.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsEnabled { get; set; }

        /// <summary>
        /// Determines whether this button is loading or not.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsLoading { get; set; }

        /// <summary>
        /// Displayed icon. Accepts a PackIconKind or a dynamic resource.
        /// </summary>
        public object Icon { get; set; }

        /// <summary>
        /// Determines whether the model will be validated before the action is executed.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object Validates { get; set; }

        /// <summary>
        /// Determines whether this action will close dialogs that host it.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object ClosesDialog { get; set; }

        /// <summary>
        /// Determines whether the model will be reset to default values before the action is executed.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsReset { get; set; }

        /// <summary>
        /// Determines whether the action has the style of a primary action.
        /// Accepts a boolean or a dynamic resource.
        /// </summary>
        public object IsPrimary { get; set; }

        /// <summary>
        /// If set to an <see cref="IActionInterceptor"/>, that object will handle the action.
        /// Accepts an <see cref="IActionInterceptor"/> or a dynamic resource.
        /// </summary>
        public virtual object Interceptor { get; set; }

        protected override FormElement CreateElement()
        {
            return new ActionElement
            {
                Action = BoundExpression.ParseSimplified(ActionName),
                ActionParameter = Parameter is string expr
                    ? BoundExpression.ParseSimplified(expr)
                    : new LiteralValue(Parameter),
                Content = Utilities.GetStringResource(Content),
                Icon = Utilities.GetIconResource(Icon),
                Validates = Utilities.GetResource<bool>(Validates, false, Deserializers.Boolean),
                ClosesDialog = Utilities.GetResource<bool>(ClosesDialog, false, Deserializers.Boolean),
                IsReset = Utilities.GetResource<bool>(IsReset, false, Deserializers.Boolean),
                IsEnabled = Utilities.GetResource<bool>(IsEnabled, true, Deserializers.Boolean),
                IsLoading = Utilities.GetResource<bool>(IsLoading, false, Deserializers.Boolean),
                IsDefault = Utilities.GetResource<bool>(IsDefault, false, Deserializers.Boolean),
                IsCancel = Utilities.GetResource<bool>(IsCancel, false, Deserializers.Boolean),
                IsPrimary = Utilities.GetResource<bool>(IsPrimary, false, Deserializers.Boolean),
                ActionInterceptor = Utilities.GetResource<IActionInterceptor>(Interceptor, null, null)
            };
        }
    }
}

using System;
using System.Collections.Generic;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.DynamicExpressions.BooleanExpressions;
using Forge.Forms.Validation;

namespace Forge.Forms.FormBuilding
{
    /// <summary>
    /// Base class for all input fields.
    /// </summary>
    public abstract class DataFormField : FormField
    {
        protected DataFormField(string key, Type propertyType)
        {
            Key = key;
            PropertyType = propertyType;
            Validators = new List<IValidatorProvider>();
            BindingOptions = new BindingOptions();
        }

        public Type PropertyType { get; }

        public IValueProvider IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the default value for this field.
        /// </summary>
        public IValueProvider DefaultValue { get; set; }

        public BindingOptions BindingOptions { get; }

        public List<IValidatorProvider> Validators { get; set; }

        public IValueProvider SelectOnFocus { get; set; }

        protected internal bool IsDirectBinding { get; set; }

        protected internal bool CreateBinding { get; set; } = true;

        protected bool StrictlyReadOnly => IsReadOnly is LiteralValue v && v.Value is true;

        protected internal override void Freeze()
        {
            const string isNotReadOnly = "IsNotReadOnly";
            base.Freeze();
            if (CreateBinding)
            {
                if (IsDirectBinding)
                {
                    Resources.Add("Value", new DirectBinding(BindingOptions, Validators));
                }
                else if (string.IsNullOrEmpty(Key))
                {
                    Resources.Add("Value", LiteralValue.Null);
                }
                else
                {
                    Resources.Add("Value", new DataBinding(Key, BindingOptions, Validators, StrictlyReadOnly));
                }
            }

            Resources.Add(nameof(IsReadOnly), IsReadOnly ?? LiteralValue.False);
            var isEnabled = IsEnabled ?? LiteralValue.True;
            var isReadonly = IsReadOnly ?? LiteralValue.False;
            var op = new AndOperator
            {
                Left = new ValueNode
                {
                    Index = 0
                },
                Right = new NotOperator
                {
                    Child = new ValueNode
                    {
                        Index = 1
                    }
                }
            };

            Resources.Add(isNotReadOnly, new MultiBooleanBinding(op, new[] { isEnabled, isReadonly }, null));
            Resources[nameof(DefaultValue)] = DefaultValue ?? LiteralValue.Null;
            Resources.Add(nameof(SelectOnFocus), SelectOnFocus ?? LiteralValue.True);
        }

        public virtual object GetDefaultValue(IResourceContext context)
        {
            if (DefaultValue != null)
            {
                return DefaultValue.GetValue(context).Value;
            }

            if (PropertyType == null || !PropertyType.IsValueType)
            {
                return null;
            }

            try
            {
                return Activator.CreateInstance(PropertyType);
            }
            catch
            {
                return null;
            }
        }
    }
}

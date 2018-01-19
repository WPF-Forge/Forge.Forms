using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using FastMember;
using Forge.Forms.Controls;
using Forge.Forms.DynamicExpressions;
using Forge.Forms.FormBuilding;

namespace Forge.Forms
{
    /// <summary>
    /// Provides utilities for bound models.
    /// </summary>
    public static class ModelState
    {
        /// <summary>
        /// Attempts to reset object properties to default values.
        /// Does nothing if object is not part of any generated form.
        /// </summary>
        public static bool Reset(object model, params string[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (properties.Length == 0)
            {
                return true;
            }

            var form = GetForms(model).FirstOrDefault();
            if (form == null)
            {
                return false;
            }

            var fields = form.DataFields;
            // Storing pairs in a dictionary to avoid duplicates.
            var existingFields = new Dictionary<string, DataFormField>();
            foreach (var property in properties)
            {
                if (fields.TryGetValue(property, out var field))
                {
                    existingFields[property] = field;
                }
            }

            Reset(model, existingFields, form.ResourceContext);
            // Update UI just in case.
            UpdateFields(model, properties);
            return true;
        }

        /// <summary>
        /// Attempts to reset object to default values.
        /// Will not work if object is not part of any generated form.
        /// </summary>
        public static bool Reset(object model)
        {
            var form = GetForms(model).FirstOrDefault();
            if (form == null)
            {
                return false;
            }

            Reset(model, form.DataFields, form.ResourceContext);
            UpdateFields(model);
            return true;
        }

        private static void Reset(object model, Dictionary<string, DataFormField> fields, IResourceContext context)
        {
            if (fields.Count == 0)
            {
                return;
            }

            var accessor = ObjectAccessor.Create(model);
            foreach (var pair in fields)
            {
                try
                {
                    var property = pair.Key;
                    var field = pair.Value;
                    if (field.DefaultValue == null)
                    {
                        var type = field.PropertyType;
                        if (type == null)
                        {
                            // No more info available to determine a value.
                            continue;
                        }

                        if (type.IsValueType)
                        {
                            accessor[property] = Activator.CreateInstance(field.PropertyType);
                        }
                        else
                        {
                            accessor[property] = null;
                        }
                    }
                    else
                    {
                        object value;
                        if (field.DefaultValue is LiteralValue literal)
                        {
                            value = literal.Value;
                        }
                        else
                        {
                            // Get proxied value.
                            value = field.DefaultValue.GetValue(context).Value;
                        }

                        accessor[property] = value;
                    }
                }
                catch
                {
                    // Tried our best.
                }
            }
        }

        /// <summary>
        /// Updates form fields with model values.
        /// Has a similar effect to <see cref="INotifyPropertyChanged" />.
        /// </summary>
        public static void UpdateFields(object model)
        {
            foreach (var expression in GetBindings(model))
            {
                expression.UpdateTarget();
            }
        }

        /// <summary>
        /// Updates form fields with property values.
        /// Has a similar effect to <see cref="INotifyPropertyChanged" />.
        /// </summary>
        public static void UpdateFields(object model, params string[] properties)
        {
            foreach (var expression in GetBindings(model, properties))
            {
                expression.UpdateTarget();
            }
        }


        /// <summary>
        /// Clear validation errors from source properties.
        /// </summary>
        public static void ClearValidationErrors(object model)
        {
            foreach (var expression in GetBindings(model))
            {
                System.Windows.Controls.Validation.ClearInvalid(expression);
            }
        }

        /// <summary>
        /// Clear validation errors from properties.
        /// </summary>
        public static void ClearValidationErrors(object model, params string[] properties)
        {
            foreach (var expression in GetBindings(model, properties))
            {
                System.Windows.Controls.Validation.ClearInvalid(expression);
            }
        }

        /// <summary>
        /// Validates source by flushing bindings.
        /// </summary>
        public static bool Validate(object model)
        {
            var hasErrors = false;
            foreach (var expression in GetBindings(model))
            {
                expression.UpdateSource();
                hasErrors = hasErrors || expression.HasValidationError;
            }

            return !hasErrors;
        }

        /// <summary>
        /// Validates source properties by flushing bindings.
        /// </summary>
        public static bool Validate(object model, params string[] properties)
        {
            var hasErrors = false;
            foreach (var expression in GetBindings(model, properties))
            {
                // The only way to validate is to attempt a write-through,
                // otherwise non-strict validation won't fire.
                expression.UpdateSource();
                hasErrors = hasErrors || expression.HasValidationError;
            }

            return !hasErrors;
        }

        internal static bool IsModel(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is ValueType || value is string)
            {
                return false;
            }

            return true;
        }

        private static List<BindingExpressionBase> GetBindings(object model, string[] properties)
        {
            var list = new List<BindingExpressionBase>();
            if (properties == null || properties.Length == 0)
            {
                return list;
            }

            foreach (var form in GetForms(model))
            {
                foreach (var property in properties)
                {
                    if (form.DataBindingProviders.TryGetValue(property, out var provider))
                    {
                        list.AddRange(provider.GetBindings());
                    }
                }
            }

            return list;
        }

        internal static BindingExpressionBase[] GetBindings(object model)
        {
            return GetForms(model)
                .SelectMany(f => f.DataBindingProviders.Values.SelectMany(p => p.GetBindings()))
                .ToArray();
        }

        private static IEnumerable<DynamicForm> GetForms(object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model is ValueType || model is string)
            {
                throw new ArgumentException("Cannot track state of primitive types.", nameof(model));
            }

            return DynamicForm.ActiveForms.Where(
                f =>
                    f.CheckAccess() // Get only synchronized controls, otherwise updating bindings would cause problems.
                    && ReferenceEquals(f.Value, model));
        }
    }
}

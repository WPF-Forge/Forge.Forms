using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Forge.Forms.Annotations;
using Forge.Forms.Extensions;
using Proxier.Extensions;
using Proxier.Mappers;
using Proxier.Mappers.Maps;

namespace Forge.Forms.Mapping
{
    public class MaterialMapper : AttributeMapper
    {
        static MaterialMapper()
        {
            ModuleInitializer.Initialize();
        }

        protected MaterialMapper(Type type) : base(type)
        {
        }

        public bool AutoHide { get; set; }

        public override object TransfomSpawn(object createInstance)
        {
            if (!AutoHide)
            {
                return base.TransfomSpawn(createInstance);
            }

            var propertyInfos = Type.GetOutmostProperties().Select(i => i.PropertyInfo);
            var shouldHave = AttributeMappings.Select(i => i.PropertyInfo).Where(i => i != null).ToList();
            foreach (var prop in propertyInfos)
            {
                if (shouldHave.Any(i => i.Name == prop.Name))
                {
                    continue;
                }

                {
                    if (AttributeMappings.Any(i => i.PropertyInfo?.Name == prop.Name))
                    {
                        continue;
                    }

                    AttributeMappings.Add(new AttributeMap(this)
                    {
                        Attributes = new Expression<Func<Attribute>>[]
                            {() => new FieldAttribute {IsVisible = false}},
                        PropertyInfo = prop
                    });
                }
            }

            return base.TransfomSpawn(createInstance);
        }

        public virtual void HandleAction(IActionContext context)
        {
        }
    }

    public class MaterialMapper<TSource> : MaterialMapper
    {
        public MaterialMapper() : base(typeof(TSource))
        {
        }

        /// <summary>
        /// Adds a mapper.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="propertyLambda"></param>
        public void AddPropertyAttribute<TProperty>(Expression<Func<TSource, TProperty>> propertyLambda,
            params Expression<Func<Attribute>>[] expression)
        {
            var type = Type;

            if (!(propertyLambda.Body is MemberExpression member))
            {
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(
                    $"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            if (propInfo.ReflectedType != null && (type != propInfo.ReflectedType &&
                                                   !type.IsSubclassOf(propInfo.ReflectedType)))
            {
                throw new ArgumentException(
                    $"Expresion '{propertyLambda}' refers to a property that is not from type {type}.");
            }

            var mapper = new AttributeMap(this)
            {
                Attributes = expression,
                PropertyInfo = propInfo
            };

            AttributeMappings.Add(mapper);
        }

        /// <summary>
        /// Called when [action happened]
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter.</param>
        public virtual void Action(TSource model, string action, object parameter)
        {
        }

        public override void HandleAction(IActionContext context)
        {
            if (context.Action is string action)
                Action((TSource) context.Model.CopyTo(BaseType.AddParameterlessConstructor()), action,
                    context.ActionParameter);
        }
    }
}
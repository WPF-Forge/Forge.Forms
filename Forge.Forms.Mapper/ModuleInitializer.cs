using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Forge.Forms.Utils;
using Ninject;
using Proxier.Mappers;

namespace Forge.Forms.Mapper
{
    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            var transformation = new Transformation
            {
                GetProperties = GetProperties,
                KernelChanged = KernelChanged,
                ModelChanged = ModelChanged,
                OnAction = OnAction
            };

            Transformation.GlobalTransformation = transformation;
        }

        private static object OnAction(object model, string actionName, object arg)
        {
            var newModel = model.GetInjectedObject()
                ?.CopyTo(model.GetType().FindOverridableType()?.BaseType ?? model.GetType());
            model.GetType().FindOverridableType<MaterialMapper>()?.HandleAction(model, actionName, arg);
            return newModel;
        }

        private static object ModelChanged(object oldModel, object newModel, IKernel kernel)
        {
            Proxier.Mappers.Mapper.InitializeMapperClasses(kernel);
            if (newModel != null)
            {
                kernel.Inject(newModel);
            }

            return newModel;
        }

        private static object KernelChanged(object o, IKernel kernel)
        {
            var newModel = o.CopyTo(o.GetType());
            kernel.Inject(newModel);
            return newModel;
        }

        private static IEnumerable<PropertyInfo> GetProperties(object o)
        {
            return o.GetType()
                .GetHighestProperties()
                .Where(p => p.PropertyInfo.CanRead && p.PropertyInfo.GetGetMethod(true).IsPublic)
                .OrderBy(p => p.Token)
                .Select(i => i.PropertyInfo);
        }
    }
}

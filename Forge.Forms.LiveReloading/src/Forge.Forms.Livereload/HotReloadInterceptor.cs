using System;
using System.Collections.Generic;
using Proxier.Extensions;

namespace Forge.Forms.Livereload
{
    internal class HotReloadInterceptor : IModelInterceptor
    {
        private static Dictionary<Type, Type> Replaces { get; }
            = new Dictionary<Type, Type>();

        public IModelContext Intercept(IModelContext modelContext)
        {
            if (modelContext.NewModel == null)
            {
                return modelContext;
            }

            if (Replaces.ContainsKey(modelContext.NewModel.GetType()))
            {
                return new ModelContext(modelContext.NewModel.CopyTo(Replaces[modelContext.NewModel.GetType()]),
                    modelContext.ResourceContext);
            }

            return modelContext;
        }

        public static void AddOrReplaceReplacement(Type type, Type targetType)
        {
            if (Replaces.ContainsKey(type))
            {
                Replaces[type] = targetType;
            }
            else
            {
                Replaces.Add(type, targetType);
            }
        }
    }
}

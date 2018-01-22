using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Forge.Forms.Controls;
using Forge.Forms.Extensions;
using Forge.Forms.FormBuilding.Defaults;
using Proxier.Mappers;

namespace Forge.Forms.Mapper
{
    class MapperInterceptor : IModelInterceptor
    {
        public IModelContext Intercept(IModelContext modelContext)
        {
            return modelContext.NewModel == null
                ? modelContext
                : new ModelContext(modelContext.NewModel.GetInjectedObject(), modelContext.ResourceContext);
        }
    }

    class MapperActionInterceptor : IActionInterceptor
    {
        public IActionContext InterceptAction(IActionContext actionContext)
        {
            if (actionContext.Model == null)
                return actionContext;

            return new ActionContext(
                actionContext.Model.CopyTo(actionContext.Model.GetType().FindOverridableType()?.BaseType ??
                                           actionContext.Model.GetType()), actionContext.Context, actionContext.Action,
                actionContext.ActionParameter);
        }
    }

    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            DynamicForm.InterceptorChain.Add(new MapperInterceptor());
            ActionElement.InterceptorChain.Add(new MapperActionInterceptor());
        }
    }
}
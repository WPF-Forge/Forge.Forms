using Proxier.Extensions;
using Proxier.Mappers;

namespace Forge.Forms.Mapper.Interceptors
{
    class MapperActionInterceptor : IActionInterceptor
    {
        public IActionContext InterceptAction(IActionContext actionContext)
        {
            if (actionContext.Model == null)
                return actionContext;

            return new ActionContext(
                actionContext.Model.CopyTo(actionContext.Model.GetType().GetMapper()?.BaseType ??
                                           actionContext.Model.GetType()), actionContext.Context, actionContext.Action,
                actionContext.ActionParameter);
        }
    }
}
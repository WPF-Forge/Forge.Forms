using Proxier.Extensions;

namespace Forge.Forms.Mapper.Interceptors
{
    /// <summary>
    /// Adds the default mapper action handler
    /// </summary>
    /// <seealso cref="Forge.Forms.IActionInterceptor" />
    class MapperActionInterceptor : IActionInterceptor
    {
        /// <summary>
        /// Intercepts the action.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns></returns>
        public IActionContext InterceptAction(IActionContext actionContext)
        {
            if (actionContext.Model == null)
                return actionContext;

            var interceptAction = new ActionContext(
                actionContext.Model.CopyTo(actionContext.Model.GetType().GetMapper()?.BaseType ??
                                           actionContext.Model.GetType()), actionContext.Context, actionContext.Action,
                actionContext.ActionParameter);

            var mapper = interceptAction.Model.GetType().GetMapper();
            if (mapper is MaterialMapper materialMapper)
            {
                materialMapper.HandleAction(interceptAction);
            }

            return interceptAction;
        }
    }
}
using System;
using Microsoft.ClearScript;

namespace Forge.Forms.Scripting
{
    internal class ScriptInterceptor : IActionInterceptor
    {
        private readonly object action;

        public ScriptInterceptor(ScriptEngine scriptEngine, string code)
        {
            action = scriptEngine.Evaluate("ScriptAction", true,
@"(function (model, context, parameter) {
  (function ScriptAction() { " + code + @" }).apply(model);
}).valueOf()");
        }

        public void InterceptAction(object model, object context, object parameter)
        {
            ((dynamic)action)(model, context, parameter);
        }
    }
}

using System;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;
using Microsoft.ClearScript.V8;

namespace Forge.Forms.Scripting
{
    public class ScriptActionAttribute : ActionAttribute
    {
        internal static readonly V8ScriptEngine ScriptEngine = new V8ScriptEngine();

        private readonly object interceptor;

        public ScriptActionAttribute(string content, string code, [CallerLineNumber] int position = 0)
            : base("[ScriptAction]", content, position)
        {
            interceptor = new ScriptInterceptor(ScriptEngine, code);
        }

        public override object Interceptor
        {
            get => interceptor;
            set => throw new InvalidOperationException("Cannot set interceptor in ScriptActionAttribute.");
        }
    }
}

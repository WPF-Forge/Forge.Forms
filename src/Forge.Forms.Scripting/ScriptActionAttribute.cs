using System;
using System.Runtime.CompilerServices;
using Forge.Forms.Annotations;

namespace Forge.Forms.Scripting
{
    public class ScriptActionAttribute : ActionAttribute
    {
        private object interceptor;

        public ScriptActionAttribute(string content, string onExecute, [CallerLineNumber] int position = 0)
            : base("[ScriptAction]", content, position)
        {
        }

        public override object Interceptor
        {
            get => interceptor;
            set => throw new InvalidOperationException("Cannot set interceptor in ScriptActionAttribute.");
        }
    }
}

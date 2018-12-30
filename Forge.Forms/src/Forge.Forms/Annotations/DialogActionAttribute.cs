using System.Runtime.CompilerServices;

namespace Forge.Forms.Annotations
{
    /// <summary>
    /// Declares an action that is only visible in dialog hosts and closes dialogs by default.
    /// </summary>
    public class DialogActionAttribute : ActionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogActionAttribute"/> class.
        /// </summary>
        /// <param name="name">Action identifier. Accepts a string or a dynamic expression.</param>
        /// <param name="content">Action content. Accepts a string or a dynamic expression.</param>
        /// <param name="position">Do not provide a value for this argument.</param>
        public DialogActionAttribute(string name, string content, [CallerLineNumber] int position = 0)
            : base(name, content, position)
        {
            IsVisible = "{Env DialogHostContext}";
            ClosesDialog = true;
        }
    }
}
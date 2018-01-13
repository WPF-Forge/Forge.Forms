using System;

namespace Forge.Forms.Forms
{
    public class ActionEventArgs : EventArgs
    {
        public ActionEventArgs(string action)
        {
            Action = action;
        }

        public string Action { get; }
    }
}

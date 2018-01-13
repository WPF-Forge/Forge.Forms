using System;

namespace Forge.Forms.Interfaces
{
    public interface IDynamicForm
    {
        object Model { get; }

        object Value { get; }

        object Context { get; }

        event EventHandler<ActionEventArgs> OnAction;
    }
}

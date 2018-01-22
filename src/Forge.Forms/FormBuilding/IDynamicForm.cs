using System;

namespace Forge.Forms.FormBuilding
{
    public interface IDynamicForm
    {
        object Model { get; }

        object Value { get; }

        object Context { get; }

        event EventHandler<ActionEventArgs> OnAction;
    }
}

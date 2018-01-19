using System;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Action("alert", "ALERT")]
    [Action("confirm", "CONFIRM")]
    [Action("prompt", "PROMPT")]
    [Action("login", "LOGIN")]
    public class Dialogs : IActionHandler
    {
        public void HandleAction(object model, string action, object parameter)
        {
            switch (action)
            {
                case "alert":
                    Show.Window(new Alert { Title = "Hello world!" });
                    break;
                case "confirm":
                    Show.Window(new Confirm { Title = "Delete item?" });
                    break;
                case "prompt":
                    Show.Window(new Prompt<string> { Title = "What's your name?" });
                    break;
                case "login":
                    Show.Window(new Login());
                    break;
                default:
                    return;
            }
        }
    }
}

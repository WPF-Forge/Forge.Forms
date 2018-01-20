using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Title("Dialogs")]
    [Action("alert", "ALERT", InsertAfter = false)]
    [Action("confirm", "CONFIRM", InsertAfter = false)]
    [Action("prompt", "PROMPT", InsertAfter = false)]
    [Action("login", "LOGIN", InsertAfter = false)]
    [Divider]
    [Title("Windows")]
    [Action("alert", "ALERT", Parameter = "window", InsertAfter = false)]
    [Action("confirm", "CONFIRM", Parameter = "window", InsertAfter = false)]
    [Action("prompt", "PROMPT", Parameter = "window", InsertAfter = false)]
    [Action("login", "LOGIN", Parameter = "window", InsertAfter = false)]
    public class Dialogs : IActionHandler
    {
        public void HandleAction(object model, string action, object parameter)
        {
            if (parameter is "window")
            {
                switch (action)
                {
                    case "alert":
                        Show.Window().For(new Alert("Hello world!"));
                        break;
                    case "confirm":
                        Show.Window().For(new Confirm("Delete item?"));
                        break;
                    case "prompt":
                        Show.Window().For(new Prompt<string> { Title = "What's your name?" });
                        break;
                    case "login":
                        Show.Window().For<Login>();
                        break;
                    default:
                        return;
                }
            }
            else
            {
                switch (action)
                {
                    case "alert":
                        Show.Dialog().For(new Alert("Hello world!"));
                        break;
                    case "confirm":
                        Show.Dialog().For(new Confirm("Delete item?"));
                        break;
                    case "prompt":
                        Show.Dialog().For(new Prompt<string> { Title = "What's your name?" });
                        break;
                    case "login":
                        Show.Dialog().For<Login>();
                        break;
                    default:
                        return;
                }
            }

        }
    }
}

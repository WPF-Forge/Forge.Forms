using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Title("Dialogs")]
    [Action("alert", "ALERT", InsertAfter = false)]
    [Action("confirm", "CONFIRM", InsertAfter = false)]
    [Action("long_confirm", "LONG CONFIRM", InsertAfter = false)]
    [Action("prompt", "PROMPT", InsertAfter = false)]
    [Action("login", "LOGIN", InsertAfter = false)]
    [Divider]
    [Title("Windows")]
    [Action("alert", "ALERT", Parameter = "window", InsertAfter = false)]
    [Action("confirm", "CONFIRM", Parameter = "window", InsertAfter = false)]
    [Action("long_confirm", "LONG CONFIRM", Parameter = "window", InsertAfter = false)]
    [Action("prompt", "PROMPT", Parameter = "window", InsertAfter = false)]
    [Action("login", "LOGIN", Parameter = "window", InsertAfter = false)]
    public class Dialogs : IActionHandler
    {
        public void HandleAction(object model, string action, object parameter)
        {
            var longConfirm = new Confirm(
                "Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.",
                "Use Google's location service?", "TURN ON SPEED BOOST", "NO THANKS");

            if (parameter is "window")
            {
                switch (action)
                {
                    case "alert":
                        Show.Window(275d).For(new Alert("Hello world!"));
                        break;
                    case "confirm":
                        Show.Window(275d).For(new Confirm("Delete item?"));
                        break;
                    case "long_confirm":
                        Show.Window(250d).For(longConfirm);
                        break;
                    case "prompt":
                        Show.Window().For(new Prompt<string> { Title = "What's your name?", Value = "User" });
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
                        Show.Dialog(275d).For(new Alert("Hello world!"));
                        break;
                    case "confirm":
                        Show.Dialog(275d).For(new Confirm("Delete item?"));
                        break;
                    case "long_confirm":
                        Show.Dialog(250d).For(longConfirm);
                        break;
                    case "prompt":
                        Show.Dialog().For(new Prompt<string> { Title = "What's your name?", Value = "User" });
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

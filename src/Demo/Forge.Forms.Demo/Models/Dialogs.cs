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
        public async void HandleAction(IActionContext actionContext)
        {
            var parameter = actionContext.ActionParameter;
            var action = actionContext.Action as string;
            var longConfirm = new Confirmation(
                "Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.",
                "Use Google's location service?", "TURN ON SPEED BOOST", "NO THANKS");

            if (parameter is "window")
            {
                switch (action)
                {
                    case "alert":
                        await Show.Window(275d).For(new Alert("Hello world!"));
                        break;
                    case "confirm":
                        await Show.Window(275d).For(new Confirmation("Delete item?"));
                        break;
                    case "long_confirm":
                        await Show.Window(250d).For(longConfirm);
                        break;
                    case "prompt":
                        await Show.Window().For(new Prompt<string> { Title = "What's your name?", Value = "User" });
                        break;
                    case "login":
                        var result = await Show.Window().For<Login>();
                        if (result.Action is "login")
                        {
                            await Show.Window(275d).For(new Alert($"Hello {result.Model.Username}!"));
                        }

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
                        await Show.Dialog(275d).For(new Alert("Hello world!"));
                        break;
                    case "confirm":
                        await Show.Dialog(275d).For(new Confirmation("Delete item?"));
                        break;
                    case "long_confirm":
                        await Show.Dialog(250d).For(longConfirm);
                        break;
                    case "prompt":
                        await Show.Dialog().For(new Prompt<string> { Title = "What's your name?", Value = "User" });
                        break;
                    case "login":
                        var result = await Show.Dialog().For<Login>();
                        if (result.Action is "login")
                        {
                            await Show.Dialog(275d).For(new Alert($"Hello {result.Model.Username}!"));
                        }

                        break;
                    default:
                        return;
                }
            }
        }
    }
}

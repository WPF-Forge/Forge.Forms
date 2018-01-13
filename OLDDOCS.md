### Basic dialogs
```cs
await WindowFactory.Alert("Hello World!").Show();
bool? result = await WindowFactory.Prompt("Delete item?").Show();
```

### Customized dialogs

![login](https://github.com/EdonGashi/WpfMaterialForms/blob/master/doc/login.png)

```cs
{
    Title = "Please log in to continue",
    PositiveAction = "LOG IN",
    Form = new MaterialForm
    {
        new StringSchema
        {
            Name = "Username",
            IconKind = PackIconKind.Account
        },
        new PasswordSchema
        {
            Name = "Password",
            IconKind = PackIconKind.Key
        },
        new BooleanSchema
        {
            Name = "Remember me",
            IsCheckBox = true
        }
    },
	Theme = DialogTheme.Light // DialogTheme.Dark
}
```
---

![settings](https://github.com/EdonGashi/WpfMaterialForms/blob/master/doc/settings.png)

```cs
{
    Title = "Settings",
    Form = new MaterialForm
    {
        new CaptionSchema
        {
            Name = "Connectivity"
        },
        new BooleanSchema
        {
            Name = "WiFi",
            IconKind = PackIconKind.Wifi,
            Value = true
        },
        new BooleanSchema
        {
            Name = "Mobile Data",
            IconKind = PackIconKind.Signal
        },
        new CaptionSchema
        {
            Name = "Device"
        },
        new NumberRangeSchema
        {
            Name = "Volume",
            IconKind = PackIconKind.VolumeHigh,
            MinValue = 0,
            MaxValue = 10,
            Value = 5
        },
        new KeyValueSchema
        {
            Name = "Ringtone",
            Value = "Over the horizon",
            IconKind = PackIconKind.MusicNote
        }
    }
}
```
---

![email](https://github.com/EdonGashi/WpfMaterialForms/blob/master/doc/email.png)

```cs
{
    Title = "Send e-mail",
    PositiveAction = "SEND",
    Form = new MaterialForm
    {
        new StringSchema
        {
            Name = "To",
            IconKind = PackIconKind.Email
        },
        new StringSchema
        {
            Name = "Message",
            IsMultiLine = true,
            IconKind = PackIconKind.Comment
        }
    }
}
```
---

![dialog](https://github.com/EdonGashi/WpfMaterialForms/blob/master/doc/dialog.png)

```cs
{
    Message = "Discard draft?",
    PositiveAction = "DISCARD"
}
```
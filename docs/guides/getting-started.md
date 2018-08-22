# Getting started

Using this library is straightforward. `DynamicForm` is a control that will render controls bound to an associated model. A model can be an object, a type, a primitive, or a custom `IFormDefinition`.

A DynamicForm has two key properties, the `Model` property, which represents the form being rendered, and the `Context` property, which allows models to access data outside of their scope, such as a selection field or action handling.

## Hosting dynamic forms

`DynamicForm` is a WPF control that hosts forms. You are free to include this control anywhere in XAML:

```xml
<forms:DynamicForm Model="{Binding Model}" />
```

As you can see above, MVVM patterns are fully supported. In this way, you can expose your data from a ViewModel, and the view for manipulating that data is automatically rendered.

If you only need to display the forms as dialogs, then you can use helper class `Forge.Forms.Show` like this:

```csharp
var result = await Show.Dialog.For<Login>();
// result.Action stores the last action of the dialog
if (result.Action is "login") {
  // result.Model stores the model instance
  string username = result.Model.Username;
  string password = result.Model.Password;
  bool rememberMe = result.Model.RememberMe;
  // do something with the values
} else {
  // "cancel" is clicked, do nothing
}
```

`Show.Dialog()` will host the form in a MaterialDesignInXAML `DialogHost` control, while `Show.Window()` will host the control in a Mahapps.Metro window.

## Building forms from classes

The easiest way to build a form is to declare a class with the inputs you need. You can use annotations to fine-tune the behavior of fields, or even add form elements such as titles, text, or actions.

In the following class we declare a `Login` model:

```csharp
[Title("Log in to continue")]
[Action("cancel", "CANCEL", IsCancel = true, ClosesDialog = true)]
[Action("login", "LOG IN", IsDefault = true, ClosesDialog = true, Validates = true)]
class Login {
  [Field(Icon = "Account")]
  [Value(Must.NotBeEmpty)]
  public string Username { get; set; }
  
  [Field(Icon = "Key")]
  [Password]
  public string Password { get; set; }
  
  public bool RememberMe { get; set; }
}
```

Most of the decorators should be self-explanatory. If we call `Show.Dialog().For<Login>()`, we will see the following dialog:

![Login](https://user-images.githubusercontent.com/12145268/44461196-2e396100-a610-11e8-9f84-384831fe338e.png)

Calling `Show.Window().For<Login>()` will show the same form, except that it will be hosted in a new window.

If we want to manually display this form in XAML, we would have to write:

```xml
<forms:DynamicForm x:Name="MyForm" Model="{Binding MyLoginModel}" />
```

Where `MyLoginModel` would be a property that returned a `Login` instance, or even the `typeof(Login)` itself.

Imperatively, this could be done via:

```csharp
MyForm.Model = new Login();
// or
MyForm.Model = typeof(Login);
```

If you are not sure if the model is a type or an instance, reading `MyForm.Value` will always return the resolved instance of the input model.

## Common form helpers

Because some simple dialogs are so common, we offer built-in helper classes for them:

- `Alert` - displays a message and a button
- `Confirmation` - displays a message and up to two buttons
- `Prompt<T>` - displays a message, an input field, and up to two buttons

Examples:

```csharp
await Show.Dialog().For(new Alert("Hello world!"));
await Show.Dialog().For(new Confirmation("Delete item?", null, "YES", "NO"));
await Show.Dialog().For(new Prompt<string> { Title = "What's your name?" });
```

![Alert](https://user-images.githubusercontent.com/12145268/44462136-d6045e00-a613-11e8-913e-c291d1fad3f6.png)
![Confirmation](https://user-images.githubusercontent.com/12145268/44462151-efa5a580-a613-11e8-9dcd-ab4a73faec01.png)
![Prompt](https://user-images.githubusercontent.com/12145268/44462306-7fe3ea80-a614-11e8-8706-d69f8cab3aae.png)


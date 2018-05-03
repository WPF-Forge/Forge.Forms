# Environments

Being able to reuse domain objects in different contexts is a powerful feature of code-first design.
To allow such polymorphic behavior for displayed forms, dynamic forms have their own "environments".
The environment of a form is simply a set of strings that represent different flags/statuses.

For example, when a form is displayed with the intention of creating a new item, you would add the flag `"add"` or `"create"` to the form's environment.
If you are displaying a form to edit an existing object, you would add the flag `"edit"` or `"update"`.

Form models can decide how to behave based on the environment by utiliziing the `{Env <Flag>}` binding expression.
This expression resolves to `true` or `false` based on the presence of the specified flag in the current environment.
To read more about bindings, refer to [dynamic resources](dynamic-resources.md).

Let's suppose we have a form to register users to our system:

```csharp
class User {
    public string Username { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}
```

Because the `Username` is likely to be a primary key in a database, we need to specify the username only when the user first registers.
If the user later decides to update their personal information, they will only be able to edit `FirstName` and `LastName` properties.

A simple solution would be creating two classes `UserWithUsername` and `UserWithoutUsername`, but this is against the domain-driven philosophy.
The more elegant solution is to customize the form based on the environment it is hosted in.

For the above example, we can listen to a `"register"` flag, which indicates that the displayed form is for the purpose of registering a new user:

```csharp
class User {
    [Field(IsVisible = "{Env register}")]
    public string Username { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}
```

The `IsVisible = "{Env register}"` causes the field to be visible only when the model is hosted within a form that contains the `"register"` flag in its environment.
In other situations that field will not be visible, which is our desired behavior.

Adding or removing flags to a form's environment is easy:

```csharp
myForm.Environment.Add("register");
bool isRegister = myForm.Environment.Has("register");
myForm.Environment.Remove("register");
myForm.Environment.Clear();
```

For more information about the API, you can check out the `IEnvironment` interface.

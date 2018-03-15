# Forge.Forms

## Introduction

Do you like code-first? Do you have a lot of forms? Then this library is for you! Forge.Forms makes it easy to create code-first forms with some nice features out of the box!        

## Code Samples

##### Installation

The first thing you'll do to get started is to install the NuGet package:

> Install-Package Forge.Forms

##### The model

Alright so you want to make a login form, you probably have a model that looks like so:

```cs
public class Login {
    public string Username { get; set; }
    public string Password { get; set; }    
}
```

then all you need is to decorate the model with [attributes](https://github.com/WPF-Forge/Forge.Forms/wiki/Annotations), like so:

```cs
public class Login {
    public string Username { get; set; }
    
    [Password]    
    public string Password { get; set; }    
}
```

it will look something like this:

![](https://github.com/WPF-Forge/Forge.Forms/blob/master/images/login-1.png?raw=true)

but currently it's not that great looking, but we can change that with some simple attributes:

```cs
[Title("Please login")]
public class Login {
    public string Username { get; set; }
    
    [Password]    
    public string Password { get; set; }    
}
```

and it will be a little bit more pleasing to the eyes, something like this:

![](https://github.com/WPF-Forge/Forge.Forms/blob/master/images/login-2.png?raw=true)

Wait, but where are my buttons? Worry not my friend, Forge.Forms is here for the rescue!

```cs
[Title("Please login")]
[Action("login","LOGIN")]
public class Login {
    public string Username { get; set; }
    
    [Password]    
    public string Password { get; set; }    
}
```

and your final product will end up like this:

##### :tada: Tada
![](https://github.com/WPF-Forge/Forge.Forms/blob/master/images/login-3.png?raw=true)

Now how do we handle it? Just implement the ```IActionHandler``` interface on you model and you're good to go.

##### Displaying
To display you have two options:

* You can add a ```<DynamicForm />``` to your view and set the ```Model``` dependency property to an instance of this class.
* You can use the [our fluent API](https://github.com/WPF-Forge/Forge.Forms/wiki/Forge.Forms#show)

And you're done! For more in-depth analysis check [out the wiki](https://github.com/WPF-Forge/Forge.Forms/wiki).

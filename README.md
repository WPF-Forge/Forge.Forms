# Dynamic Forms


[![Build status](https://ci.appveyor.com/api/projects/status/5y0t43qaee7fq8iv?svg=true)](https://ci.appveyor.com/project/EdonGashi/wpfmaterialforms) [![NuGet](https://img.shields.io/nuget/dt/WpfMaterialForms.svg?style=flat-square)](https://www.nuget.org/packages/WpfMaterialForms/)





This library is in early stages of being reworked to support dynamically generated forms in MVVM style:

```xaml
<Grid>
    <DynamicForm Model="{Binding CurrentUser}" />
</Grid>
```

```cs
public class ViewModel
{
    public User CurrentUser { get ... }
    
    public List<string> Users { get ... }
    
    public bool RequireLongPasswords => true;
}

public class User
{
    [Field(Name = "First Name",
        Tooltip = "Enter your first name here.",
        Icon = PackIconKind.Account)]
    [Value(Must.NotBeEmpty)]
    public string FirstName { get; set; }

    [Field(Name = "Last Name",
        Tooltip = "Enter your last name here.")]
    [Value(Must.NotBeEmpty)]
    public string LastName { get; set; }

    [Value(Must.BeLessThan, "2020-01-01",
        Message = "You said you are born in the year {Value:yyyy}. Are you really from the future?")]
    public DateTime DateOfBirth { get; set; }

    [Field(Name = "Username")]
    [Value(Must.MatchPattern, "^[a-zA-Z][a-zA-Z0-9]*$",
        Message = "{Value} is not a valid username, usernames must match pattern {Argument}.")]
    [Value(Must.NotExistIn, "{ContextBinding Users}",
        Message = "User {Value} is already taken.")]
    public string Username { get; set; }

    [Value("Length", Must.BeGreaterThan, 6,
        Message = "Your password has {Value|Length} characters, which is less than the required {Argument}.")]
    [Value("Length", Must.BeGreaterThan, 12,
        When = "{ContextBinding RequireLongPasswords}",
        Message = "The administrator decided that your password must be really long!")]
    public string Password { get; set; }

    [Value(Must.BeEqualTo, "{Binding Password}",
        Message = "The entered passwords do not match.")]
    public string PasswordConfirm { get; set; }

    [Value(Must.BeTrue, Message = "You must accept the license agreement.")]
    public bool AgreeToLicense { get; set; }
}
```

Result, depending on app theme:
![user](https://github.com/EdonGashi/WpfMaterialForms/blob/master/doc/user.png)

# WPF Material Forms

[NuGet](https://www.nuget.org/packages/WpfMaterialForms) - ```Install-Package WpfMaterialForms```

A windows and dialogs library using Material Design In XAML Toolkit and MahApps.Metro.

The dialogs and windows are generated dynamically from data schemas. The API is aimed to be detached from XAML/WPF and the underlying libraries.

Check out MaterialForms.WpfDemo for easy to follow examples.

## Examples

*[If you're looking for the Old documentation click here](https://github.com/redbaty/WpfMaterialForms/blob/master/OLDDOCS.md)*

Use attributes to describe how controls should be rendered like this

```csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MaterialForms.Wpf.Annotations;

namespace MaterialForms.Demo.Models
{
    public class FoodSelection : INotifyPropertyChanged
    {
        private string firstFood = "Pizza";
        private string secondFood = "Steak";
        private string thirdFood = "Salad";
        private string yourFavoriteFood;

        [Field(DefaultValue = "Pizza")]
        [Value(Must.NotBeEmpty)]
        public string FirstFood
        {
            get => firstFood;
            set
            {
                firstFood = value;
                OnPropertyChanged();
            }
        }

        [Field(DefaultValue = "Steak")]
        [Value(Must.NotBeEmpty)]
        public string SecondFood
        {
            get => secondFood;
            set
            {
                secondFood = value;
                OnPropertyChanged();
            }
        }

        [Field(DefaultValue = "Salad")]
        [Value(Must.NotBeEmpty)]
        public string ThirdFood
        {
            get => thirdFood;
            set
            {
                thirdFood = value;
                OnPropertyChanged();
            }
        }

        [Text("You have selected {Binding YourFavoriteFood}", InsertAfter = true)]

        [SelectFrom(new[] { "{Binding FirstFood}, obviously.", "{Binding SecondFood} is best!", "I love {Binding ThirdFood}" })]
        public string YourFavoriteFood
        {
            get => yourFavoriteFood;
            set
            {
                yourFavoriteFood = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

for more information click on one of this attributes
* Binding
* Field
* SelectFrom
* Value


[See more about attributes](https://github.com/redbaty/WpfMaterialForms/wiki/Attributes)

## How to use
### In a WPF project

In your App.xaml you need to have the following resources included. If you are using Material Design in XAML for you UI you will have those already declared (the color theme does not matter).
```xaml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml" />
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml" />
            <ResourceDictionary Source="pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Yellow.xaml" />
	    <!-- If you're going to use SelectFrom -->
	    <ResourceDictionary Source="pack://application:,,,/MaterialForms;component/Themes/Material.xaml" />
	    
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Outside of WPF (WinForms, Console)
You need to call this method only once before creating any Material window or dialog (in Main or somewhere during startup):
```cs
MaterialApplication.CreateApplication();
```

If you need a message loop you can call ```MaterialApplication.RunDispatcher();```

Before stopping your application you need to shut down explicitly:
```cs
MaterialApplication.ShutDownApplication();
```

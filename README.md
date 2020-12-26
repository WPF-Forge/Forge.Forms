# Forge.Forms

[![Build status](https://ci.appveyor.com/api/projects/status/dgimwxv2wlkh7go3/branch/master?svg=true)](https://ci.appveyor.com/project/EdonGashi/forge-forms/branch/master)
![NuGet](https://img.shields.io/nuget/v/Forge.Forms.svg)


## Introduction

With Forge.Forms you can create dynamic forms in WPF from classes or XML.

This repo contains the latest (runable) Metro Apps and MaterialDesign Themes NuGet packages as well as their referenced libraries. However, please consider this repo to be experimental.

Join us at https://gitter.im/WPF-Forge for questions or general discussion.

## Getting started

Read our [getting started](https://wpf-forge.github.io/Forge.Forms/guides/getting-started) guide.

## Installation

```
Install-Package Forge.Forms
```

For material theme, add this to App.xaml

```xml
<ResourceDictionary Source="pack://application:,,,/Forge.Forms;component/Themes/Material.xaml" />
```

### DynamicForm control

If you want to use `DynamicForm`, import this namespace in XAML:

```
xmlns:forms="clr-namespace:Forge.Forms.Controls;assembly=Forge.Forms"
```

And use the control:

```xml
<forms:DynamicForm Model="{Binding Model}" />
```

### Displaying dialogs

If you only need to show windows and dialogs, use the `Show` helper:

```csharp
using Forge.Forms;

await Show.Dialog().For<Login>();
await Show.Window().For(new Alert("Hello world!"));
```

Note: if you are using `Show.Dialog()` without specifying a dialog identifier, it expects you to have a `DialogHost` in your XAML tree.

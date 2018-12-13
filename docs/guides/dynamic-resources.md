# Dynamic Resources

One limitation with .NET attributes is that they require values that can be evaluated at compile time. Due to this limitation, it is not possible to pass objects or dynamic values to annotations.

To allow flexible declarations while keeping the simplicity of attributes, this library introduces the concept of dynamic resources. Dynamic resources bring the power of WPF bindings to a simple and lightweight syntax, which can be used inside attributes.

A dynamic resource is any value that can change while the application is running. Such values can come from different sources, for example the value of another property, a resource from the app resource dictionary, or even the contents of a file.

## Resource Types

Forge.Forms currently supports the following data sources:

- `{Binding <Property>}` - Gets the value of the specified property of current form model, and updates automatically when that value changes.
- `{Property <Property>}` - Gets the value of the specified property of current form model, but does not update when that value changes.
- `{ContextBinding <Property>}` - Gets the value of the specified property of current form context, and updates automatically when that value changes.
- `{ContextProperty <Property>}` - Gets the value of the specified property of current form context, but does not update when that value changes.
- `{DynamicResource <ResourceKey>}` - Gets the value of the specified resource key, and updates automatically when that value changes.
- `{StaticResource <ResourceKey>}` - Gets the value of the specified resource key, but does not update when that value changes.
- `{FileBinding <FilePath>}` - Reads the text content of specified file path, and updates automatically when the file content changes.
- `{File <FilePath>}` - Reads the text content of specified file path, but does not update when the file content changes.
- `{Env <EnvFlag>}` - Returns whether the specified flag key exists in current form environment, and updates automatically if that flag is added or removed.
- Contextual resources such as `{Value}` and `{Argument}` inside validators.

## Syntax Specification

A dynamic resource is declared as follows, where brackets indicate optional content:

`{[^]<ResourceType> [']<ResourcePath>['][,<Padding>][:<StringFormat>][|<Converter>]}`

The optional leading caret `^` indicates that the contextual resource should not be synchronized with the source. For example `{^Env ReadOnly}` checks for the presence of the `ReadOnly` flag only when the form is constructed, and will not return an updated value even if that flag is later added.

`ResourceType` can be one of the listed resource types above.

`ResourcePath` specifies the argument of the resource type, such as the name of a property or the key of a resource. The resource path can be wrapped in single quotes, which is useful if the path has unconventional characters, e.g. `{File 'C:/Program Files/my file.txt'}`

`Padding` can be optionally added, which is applicable when the value is represented as a string. For example `{Binding Name,-30}` will display the `Name` property using 30 characters and will be padded to the left. A positive value indicates right padding.

`StringFormat` can be optionally added, which is applicable when the value is represented as a string. This parameter will be passed to the `ToString(format)` method. For example, `{Binding Year:yyyy}` displays the DateTime property `Year` in `yyyy` format.

`Converter` specifies the value converter to be used in the established binding. If a custom converter needs to be used, it must be registered first. Example: `{Binding Name|ToUpper}` converts the `Name` property to upper case.

The list of default converters is given below:

- `IsNull` - returns whether the provided value is null.
- `IsNotNull` - returns whether the provided value is not null.
- `AsBool` - returns whether the provided value is `true`.
- `Negate` - returns whether the provided value is `false`.
- `IsEmpty` - returns whether the provided string or collection has length 0.
- `IsNotEmpty` - inverse of IsEmpty.
- `ToUpper` - converts the provided string to upper case.
- `ToLower` - converts the provided string to lower case.
- `Length` - returns the string or collection length.
- `ToString` - calls `ToString()` on the provided value.
- `IsEqual(arg)` - returns whether the provided value is equal to `arg`. `arg` can only be a literal value. Examples: `IsEqual(true)`, `IsEqual(false)`, `IsEqual(2.5)`, `IsEqual('Hello')`.
- `ToVisibility` - attempts to convert the provided value to a WPF visibility value.
- `HideOnFalse` - similar to `ToVisibility`, except that false values generate `Visibility.Hidden`.

You can add custom converters in `Forge.Forms.DynamicExpressions.Resource.ValueConverters` dictionary.

If the requested converter can not be found from the above collection, it is looked up as a WPF resource relative to the displayed form.

## Dynamic Expressions

Dynamic expressions are an extension to dynamic resources, which allow the declaration of multi-bindings seamlessly. For example, the following is considered a dynamic expression:

```
Hello {Binding FirstName} {Binding LastName|ToUpper}, it is the year {DynamicResource CurrentYear:yyyy}.
```

The above expression will update automatically when any of the dependent resources changes!

Dynamic expressions make sense only when the resulting value will be represented as a string. They are extremely powerful when used in error messages, labels, or tooltips.

## Boolean Expressions

As with templated strings, it is particularly useful to combine boolean values into a single value. Forge.Forms ships with a mini-parser that can read boolean expressions. Any property that is a boolean and that accepts a dynamic resource will also accept a boolean expression.

For example `IsVisible = "!{Binding FirstName|IsEmpty} && {Binding AgreeToLicense}"`. This expression evaluates to true only when FirstName is NOT empty AND AgreeToLicense is true.

The parsed grammar includes the standard boolean operators: `||` represents OR operator, `&&` represents AND operator and takes precedence over OR, `!` represents negation, and has the highest priority. Parentheses can be used to specify a different priority of operators.

## Escaping curly braces

If you need to output a literal curly brace, you need to escape it by typing the same character twice. For example `}}` represents a literal `"}"`.

If you need a verbatim string without parsing for resources at all, you can start the string with a `@` character. For example `@This will print {Binding Name}` will result in the literal string `"This will print {Binding Name}"`

If you need a string that begins with the `@` character, you need to escape it with a leading `\`. Example `\\@Hello {Binding Name}!` produces string `"@Hello John!"`

If you need a verbatim string that begins with the `@` character, you prepend an additional `@`. Example `@@Hello {Binding Name}` produces the string `"@Hello {Binding Name}"`

If you need a string that begins with a `\`, you need to add a second leading `\`. Because a `\` must be itself escaped as `"\\"`, it becomes quite confusing. Example `\\\\Hello` results in string `"\\Hello"`, which is actually `\Hello` when printed.

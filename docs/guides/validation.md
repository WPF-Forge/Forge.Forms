# Validation

WPF offers a few approaches to validation, such as adding validation rules to bindings, or by implementing interfaces such as `INotifyDataErrorInfo` or `IDataErrorInfo`.

Because validation has an essential role in forms, we tried to make it simple and declarative.
The WPF way did not fit with the theme of this library, which is why we added a unified and decoration-based approach.

All validation rules are declared via the `ValueAttribute`. Declaring a `ValueAttribute` requires providing its validation type via the `Must condition` parameter. You are allowed to add multiple attributes to the same property, in which case all of them must be satisfied for the property to be considered valid.

With a few examples you will understand the idea behind this, and your IDE auto-complete will assist you in finding the right conditions for your properties.

```csharp
[Value(Must.NotBeEmpty)]
public string FirstName { ... }

[Value(Must.NotBeEmpty)]
public string LastName { ... }

[Value(Must.BeLessThan, "2020-01-01",
    Message = "You said you are born in the year {Value:yyyy}. Are you really from the future?")]
public DateTime? DateOfBirth { ... }

[Value(Must.MatchPattern, "^[a-zA-Z][a-zA-Z0-9]*$",
    Message = "{Value} is not a valid username, usernames must match pattern {Argument}.")]
public string Username { ... }

[Value("Length", Must.BeGreaterThanOrEqualTo, 6,
    Message = "Your password has {Value|Length} characters, which is less than the required {Argument}.")]
public string Password { ... }

[Value(Must.BeEqualTo, "{Binding Password}",
    Message = "The entered passwords do not match.",
    ArgumentUpdatedAction = ValidationAction.ClearErrors)]
public string ConfirmPassword { ... }
```

You can find more examples in the [demo app](https://github.com/WPF-Forge/Forge.Forms/tree/master/Forge.Forms/src/Forge.Forms.Demo/Models).

## Conditionally enabling a validator

If you need to conditionally enforce a validation rule you can use the `When` property, which accepts `true`/`false` or a [boolean expression](dynamic-resources.md#boolean-expressions).
Example:

```csharp
[Value("Length", Must.BeGreaterThan, 12,
    When = "{ContextBinding RequireLongPasswords}",
    Message = "The administrator decided that your password must be really long!")]
public string Password { ... }
```

## Customizing error messages

As mentioned in [dynamic resources](dynamic-resources.md#dynamic-expressions), validation error messages support dynamic expressions.
Besides the default resource types, inside error messages you also have access to `{Value}` and `{Argument}` contextual resources. `{Value}` refers to the value being validated. `{Argument}` refers to the argument passed to the declaring `ValueAttribute`.

For example, in the validation below, if the user enters the value 10, `{Value}` will be 10 and `{Argument}` resolves to 15.

```csharp
[Value(Must.BeGreaterThan, 15, Message = "You entered {Value}, which is not greater than {Argument}.")]
public int MyNumber { ... }
```

You may have hardcoded the 15 inside the message string itself, but it is useful in situations where you don't have the value available immediately:

```csharp
[Value(Must.BeGreaterThan, "{ContextBinding MinimumValue}",
    Message = "You entered {Value}, which is not greater than {Argument}.")]
public int MyNumber { ... }
```

## Validating computed values

If you need to validate a computed value instead of the raw input, you can specify a value converter as the first parameter:

```csharp
[Value("Length", Must.BeGreaterThanOrEqualTo, 6)]
public string Password { ... }
```

The above can be thought of as validating `{Value|Length}`.
Converters are resolved by name using the same logic as in [dynamic resources](dynamic-resources.md#syntax-specification).

## Update triggers

There are two properties that specify behavior when value or the argument changes.

`ValidatesOnTargetUpdated` - determines whether changing the property value will trigger the validator.
By default validation is triggered only when the property is modified from field controls.

`ArgumentUpdatedAction` - what will happen if the `{Argument}` value changes? There are three possible options:

- DoNothing - do nothing.
- ValidateField - trigger re-validation with updated argument value.
- ClearErrors - remove validation errors from the field. This means that the field will wait for further input to re-validate.

## Strict validation

There are two types of validation: strict and non-strict. Strict validation is set via the `StrictValidation` property. Validators are non-strict by default.

Strict validation means that values which don't pass validation are prevented from being written to the property.

For example:

```csharp
[Value(Must.BeGreaterThan, 15, StrictValidation = true)]
public int MyNumber { ... }
```

If the user types the number 10, then that value will not be written to `MyNumber` property, because the validation is strict.
If it were non-strict then 10 would have been written despite the validation rule failing.

Note: Writing to the property is not always possible even with non-strict validation.
If the user would have typed `"Hello"` inside the field, conversion to a number would fail.
Data conversion is not in the scope of validation, therefore it is customized using the `[Binding(ConversionErrorMessage = "...")]` attribute.

## Note on dynamic expressions

Keep in mind that binding syntax is parsed inside arguments. If you need to specify a regular expression validation, make sure to properly [escape](dynamic-resources.md#escaping-curly-braces) your curly braces.

## List of validation types

- Must.BeEqualTo - Property must equal to a value.
- Must.NotBeEqualTo - Property must not equal to a value.
- Must.BeGreaterThan - Property must be greater than a value.
- Must.BeGreaterThanOrEqualTo - Property must be greater than or equal to a value.
- Must.BeLessThan - Property must be less than a value.
- Must.BeLessThanOrEqualTo - Property must be less than or equal to a value.
- Must.BeEmpty - Property must be empty. A string is empty if it is null or has length 0. A collection is empty if it is null or has 0 elements.
- Must.NotBeEmpty - Property must not be empty. A string is empty if it is null or has length 0. A collection is empty if it is null or has 0 elements.
- Must.BeTrue - Property must be true.
- Must.BeFalse - Property must be false.
- Must.BeNull - Property must be null.
- Must.NotBeNull - Property must not be null.
- Must.ExistIn - Property must exist in a collection.
- Must.NotExistIn - Property must not exist in a collection.
- Must.MatchPattern - Property must match a regex pattern.
- Must.NotMatchPattern - Property must not match a regex pattern.
- Must.SatisfyMethod - Property value must satisfy model's static method of signature `public static bool <Argument>(ValidationContext)`. Throws if no such method is found.
- Must.SatisfyContextMethod - Property value must satisfy context's static method of signature `public static bool <Argument>(ValidationContext)`. Does nothing if no such method is found.

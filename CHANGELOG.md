# Changelog

## 1.0.20

- `Prompt<T>` now validates input on positive action.
- Metro dialog window no longer forces controls theme.
- Added ComboBox control to WPF theme.
- Added `IsDefault` and `IsCancel` bindings in WPF and Metro actions.

## 1.0.19

- Added support for `IsEnabled` in attributes or `enabled` in XML for input elements. 

## 1.0.18

- Fixed bug with bound expressions not escaping curly braces properly.

## 1.0.17

- Added FormBuilder.TypeConstructors that allows registering custom factories for custom XML input `type` attributes.
- Added TimePicker control
    - You can add it by decorating `DateTime` class properties with the `[Time]` attribute. Property `Is24Hours` can be assigned to a boolean or a dynamic resource.
    - You can add it via `<input type="time" ... />` in XML. Attribute `is24hours` can be assigned to a boolean or a dynamic resource.
- Fixed `MaterialColoredIconStyle` validation brush fill for invalid controls.

## 1.0.16

- Added error text element.
    - You can add it via `[ErrorText]` attribute in classes.
    - You can add it via `<error>` tag in XML.

## 1.0.15

- Added parameterized value converters which are created from factories registered in `Resource` class.
- Non-input XML elements accept `visible` property.
- Added `Invalidate`, `IsValid`, `ValidateWithoutUpdate`, `GetValidationErrors` utilities in ModelState class.
    - `Invalidate` temporarily fails validation for a property. If you modify the field, call `Validate`, or call `ValidateWithoutUpdate`, the marked error will disappear. To check the state of this validation error use `IsValid`/`GetValidationErrors`.
    - `IsValid` is useful if you want to check current validation state without triggering side-effects like `Validate`, which sometimes produces unexpected results.
    - `ValidateWithoutUpdate` should generally be avoided unless you are dealing with some strange edge cases.
    - `GetValidationErrors` returns the list of error strings from specified properties/whole model.
- Added `Must.BeInvalid` (and `Must.Fail` alias) validator for external validation. This validator always fails, so you must combine it with the `When` condition.
- Added `ValueAttribute.OnActivation` and `ValueAttribute.OnDeactivation` validation actions which specify what happens when the validator is enabled/disabled from the `When` condition.
- Added `SelectionType.RadioButtonsInline` for horizontal layout of radio buttons in selections.

## 1.0.14

- Stable NuGet release.

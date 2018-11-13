# Changelog

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

## 1.0.14

- Stable NuGet release
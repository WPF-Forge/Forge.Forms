# Annotations

Forms created from classes can be customized by decorating respective properties with custom annotations, which are located in `Forge.Forms.Annotations` namespace.

**Note**: In the documentation below, the term `dynamic expression` or `dynamic resource` refers to binding expressions, which are explained in [[Dynamic Bindings|Dynamic Bindings]]

---

## [Field]

Indicates that the property is a form field and allows specifying its details. In case `DefaultFields` is set to `None`, adding the `[Field]` attribute will cause the field to be generated.

This attribute has properties for customizing the look and contents of a generated field.

#### [Field(Name = "...")]

The display name of the field. This will usually affect the label of the generated control. For example the floating label of a textbox, or the text of a checkbox.

When `Name` is not set, the property name is used as the control label, which is converted to sentence case using the [Humanizer package](https://github.com/Humanizr/Humanizer#humanize-string).

Accepts a `string` or a `dynamic expression`.

#### [Field(ToolTip = "...")]

The tooltip of the field which shows on hover. It is not guaranteed that all control types will display their tooltip on hover.

Accepts a `string` or a `dynamic expression`.

#### [Field(Icon = ...)]

The icon associated with the field. Not all field types may support icons.

There are three configurations for icons:

- A displayed icon. When you want to display an icon, assign a `PackIconKind` value, or a `dynamic resource` which resolves to a `PackIconKind`.
- No icon, space not reserved (`Visibility.Collapsed`). When the `Icon` property is not specified, is assigned to `null`, or is assigned to `dynamic resource` which eventually resolves to null, the generated control will not display an icon, and no space will be calculated in layout. Think of this as `Visibility.Collapsed`.
- No icon, space reserved (`Visibility.Hidden`). When the `Icon` is assigned to the string `"None"`, no icon will be displayed, but space will be reserved as if an icon exists. This would be useful when you need to align the control with other controls that have icons.

Accepts a `PackIconKind` or a `dynamic resource`.

#### [Field(IsVisible = ...)]

Determines whether this field will be visible. Fields will never display if they are not generated, so this only applies when the field is generated and its layout is calculated. This is powerful when combined with binding syntax to conditionally display a control.

Accepts a `boolean` or a `dynamic resource`.

#### [Field(IsReadonly = ...)]

Determines whether the field is editable. This property has effect only when the decorated property has a public setter, otherwise the field will always be readonly.

Accepts a `boolean` or a `dynamic resource`.

#### [Field(DefaultValue = ...)]

Determines the default value of this field. This value is applied when `ModelState.Reset` is called, or an `Action` with `IsReset = true` is clicked.

Some types such as DateTime and numbers can be deserialized from strings. Conversions from strings are done using invariant culture. Dynamic expressions can also be used as default values.

The reset process is best-effort, which means that the attribute user is responsible for specifying a legal value.

Accepts an `object` of the same type as the property type, or a `dynamic expression`.

#### [Field(Position = ...)]

Determines the relative position of this field in the form. Fields are sorted based on this value, which has a default value of 0.

Accepts an `integer` only.

#### [Field(Row = ...)]

Specifies the row name. Fields sharing the same row name will be aligned in columns when possible.

Example:

```csharp
[Field(Row = "Reservation"]
public DateTime ReservationDate { get; set; }

[Field(Row = "Reservation"]
public int NumberOfSeats { get; set; }
```

In the example above, both controls will share the same row.

Accepts a `string` only.

#### [Field(Column = ...)]

Specifies the column number. Applicable only when `Row` property is set.

Accepts an `integer` only.

#### [Field(ColumnSpan = ...)]

Specifies the column span. Applicable only when `Row` property is set.

Accepts an `integer` only.

---

## [FieldIgnore]

Properties marked with this attribute will never be generated.

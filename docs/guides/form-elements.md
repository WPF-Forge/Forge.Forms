# Form Elements

Besides input fields, which are declared through regular properties, additional form content can be added by using annotations.

## Text elements

There are three types of text elements: Title, Heading, and Text.

- `[Title]` - displays large and bold text, usually representing the form title.
- `[Heading]` - displays accented text to denote a section or subheading.
- `[Text]` - displays regular sized text.

Font sizes are controlled by the following attached properties:

- `TextProperties.TitleFontSize`
- `TextProperties.HeadingFontSize`
- `TextProperties.TextFontSize`

These properties are inheritable in the XAML tree. As such, you can customize these values in the hosting `DynamicForm` or any of its parents.

## Actions

Actions render as buttons. They are added through the `[Action]` attribute. Actions are discussed separately in the [next page](actions.md).

## Other elements

- `[Break]` - adds a small gap between controls. You can specify the break height.
- `[Divider]` - adds a horizontal line between controls.
- `[Image]` - adds an image element. Has layout properties that correspond to WPF's Image control.
- `[Card]` - wraps X following rows in a card.

## Element placement

You can only decorate a property by specifying the attribute above it. You can customize this behavior by setting `Placement` property, which can have one of these values:

- `Placement.Before` - element is added in a row before the field/form.
- `Placement.After` - element is added in a row after the field/form.
- `Placement.Inline` - element is added inline with the field. Does not work with form decorator.

If you add attributes at class level, the content is either placed before or after the form, depending on `Placement` value.

`ShareLine` property indicates whether the element accepts following elements to be inlined in the same row. `LinePosition` is another property that specifies the element's horizontal alignment in its row. Possible values are `LinePosition.Left` and `LinePosition.Right`.

## Custom elements

You can create your own content by subclassing `FormContentAttribute`, and providing an implementation to `FormElement CreateElement()`, where you must return an implementation of `FormElement`.

After this step is done, you can attach the attribute to your form/fields like you would any of the above.
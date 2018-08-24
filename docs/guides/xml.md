# XML Forms

Forms can be created from XML strings. An example:

```csharp
IFormDefinition myDefinition = FormBuilder.Default.GetDefinition(xmlstring);
```

This definition can be used as `DynamicForm`'s `Model` value. The `DynamicForm.Value` property will contain the form's instance, which will be a dynamic object.

## Input elements

Input fields are declared as follows:

```xml
<input
  name="MyInput"
  type="string"
  label="My input's label"
  defaultValue="Enter value here"
  visible="true"
  readonly="false"
  icon="Account"
  tooltip="Enter your account name in this field."
  stringformat="..."
  conversionCulture="..."
  conversionError="..."
  updateSourceTrigger="...">
  <validate must="NotBeEmpty" message="Please enter a value." />
</input>
```

More documentation for XML in the near future.

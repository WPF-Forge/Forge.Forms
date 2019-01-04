# Actions

Actions are closely tied to forms and dialogs. They are declared by attaching `[Action]` attributes at desired locations.

## Adding actions

Actions are added through the `[Action]` attribute. Actions have these properties:

- `ActionName` - a string that identifies the action. For example, `"submit"` and `"cancel"` are common values.
- `Content` - text content of rendered button.
- `Parameter` - any parameter you want to pass to the action handler. Accepts [dynamic resources](dynamic-resources.md).
- `IsEnabled` - if this resolves to false, the button will be grayed out.
- `IsLoading` - while this resolves to true, the button will be replaced by a loading indicator.
- `Icon` - icon inside the action button.
- `Validates` - indicates whether the form should be validated when the action is performed. If validation fails the event will not propagate to handlers.
- `ClosesDialog` - indicates whether the action will close its hosting dialog.
- `IsReset` - indicates whether the action will reset the model to its default state.
- `IsPrimary` - indicates whether the action will display as a primary (raised) button.
- `IsDefault` - indicates whether the action can be performed with Enter key.
- `IsCancel` - indicates whether the action can be performed with Escape key.

Properties regarding layout are discussed in [form elements](form-elements.md).

## Dialog actions

It is common to use actions only when the form is hosted in a dialog. For this reason you can use `[DialogAction]` attribute. DialogAction has `IsVisible = {Env DialogHostContext}` and `ClosesDialog = true` by default.

## Handling actions

When an action is performed, an `IActionContext` is created and is passed to:

- Global action interceptors, which are registered in `ActionElement.InterceptorChain`.
- Local interceptor, which is set to `ActionElement.ActionInterceptor`.
- The form model, in case it implements `IActionHandler`.
- The form context, in case it implements `IActionHandler`.

`IActionContext` contains the following members:

- `Model` - model instance that contains the performed action.
- `Context` - form context at the time of invocation.
- `Action` - action identifier, usually `ActionAttribute.ActionName`.
- `ActionParameter` - resolved value of `ActionAttribute.Parameter`.
- `ResourceContext` - form's associated `IResourceContext`. Usually this will also be an `IFrameworkResourceContext` which you can use to resolve the `DynamicForm` hosting the form.
- `CloseFormHost()` - closes form host when invoked. Useful when you have asynchronous action handling.

As noted above, both model and form context can handle actions. As a guideline, we suggest this pattern:

- Model instances should handle actions if they are directly related to their state, e.g. reset or validate fields.
- Context instances should handle actions that manage state outside of the model scope, e.g. save submitted form to database.
- Interceptors should listen to actions for logging, syncing with other plugins, etc.

## Intercepting actions

You can write custom plugins or reusable logic and attach them to `ActionElement.InterceptorChain` or `ActionElement.ActionInterceptor`. An `IActionInterceptor` must implement `IActionContext InterceptAction(IActionContext actionContext)` which takes the current action context and can do things like:

- Perform side effects based on the action context.
- Return `null` to cancel action propagation to other handlers.
- Return a different `IActionContext` to allow proxying or injection.
- Return the original unmodified `IActionContext` and only "tap" the action event for side effects.

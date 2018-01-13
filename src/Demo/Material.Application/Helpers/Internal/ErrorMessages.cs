namespace Material.Application.Helpers
{
    internal static class ErrorMessages
    {
        public const string MustHaveRoutes = "Current object must be associated with a route stack.";

        public const string RoutesAssociatedWithOtherStack = "Route is already associated with another stack.";

        public const string MustBeActiveRoute =
            "Cannot initiate route transitions if current object is not the active route.";

        public const string MainWindowNotFound = "No window has been shown for current controller.";
    }
}

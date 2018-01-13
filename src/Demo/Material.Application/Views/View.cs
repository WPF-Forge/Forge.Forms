namespace Material.Application.Views
{
    public static class View
    {
        public static object Loading() => new LoadingView(null);

        public static object Loading(string message) => new LoadingView(message);
    }
}

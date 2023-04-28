namespace SogetiTODO.Contracts;

public class Routes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    public static class Home
    {
        private const string HomeBase = Base + "/Home";
        public const string Index = HomeBase + "/Index";
    }

    public static class Todo
    {
        private const string TodoBase = Base + "/Todo";
        public const string Index = TodoBase + "/Index";
        public const string Add = TodoBase + "/Add";
        public const string Update = TodoBase + "/Update";
        public const string Delete = TodoBase + "/Delete";
    }
}
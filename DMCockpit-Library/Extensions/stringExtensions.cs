namespace DMCockpit_Library.Extensions
{
    public static class stringExtensions
    {
        public static string MinifyJS(this string js)
        {
            return js.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
        }
    }
}

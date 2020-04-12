namespace Ase
{
    public static class Extensions
    {
        public static string ToStringOr(this object value, string ifNull) =>
            value == null
                ? ifNull
                : value.ToString();

        public static int ToInt(this string text, int @default = 0) =>
            int.TryParse(text, out int result) 
                ? result 
                : @default;
    }
}
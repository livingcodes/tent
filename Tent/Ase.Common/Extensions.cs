namespace Ase
{
    public static class Extensions
    {
        public static string ToStringOr(this object value, string ifNull) {
            return value == null
                ? ifNull
                : value.ToString();
        }

        public static int ToInt(this string text, int @default = 0) {
            int result = @default;
            var succeeded = int.TryParse(text, out result);
            return succeeded ? result : @default;
        }
    }
}
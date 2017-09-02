namespace Tent
{
    public static class Extensions
    {
        public static string ToStringOr(this object value, string ifNull) {
            return value == null
                ? ifNull
                : value.ToString();
        }
    }
}
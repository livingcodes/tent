namespace Tent.Data
{
    public interface ITableName
    {
        string Get<T>();
        string Get(object instance);
    }
    public class TableName_AddLetterS : ITableName
    {
        public string Get<T>() => typeof(T).Name + "s";
        public string Get(object instance) => instance.GetType().Name + "s";
    }
    public class TableName_ClassName : ITableName
    {
        public string Get<T>() => typeof(T).Name;
        public string Get(object instance) => instance.GetType().Name;
    }
}
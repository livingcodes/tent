namespace Tent.Data
{
    /// <summary>Define convention database table name</summary>
    public interface ITableName
    {
        /// <summary>Get table name that stores instances of the specified type</summary>
        string Get<T>();
        /// <summary>Get table name that stores instance</summary>
        string Get(object instance);
    }

    /// <summary>Table name equals type name with letter s appended</summary>
    public class TableName_AddLetterS : ITableName
    {
        public string Get<T>() => typeof(T).Name + "s";
        public string Get(object instance) => instance.GetType().Name + "s";
    }

    /// <summary>Table name equals type name</summary>
    public class TableName_ClassName : ITableName
    {
        public string Get<T>() => typeof(T).Name;
        public string Get(object instance) => instance.GetType().Name;
    }
}
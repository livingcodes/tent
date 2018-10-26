﻿namespace Ase
{
    /// <summary>Table name equals type name</summary>
    public class TableName_ClassName : ITableName
    {
        public string Get<T>() => typeof(T).Name;
        public string Get(object instance) => instance.GetType().Name;
    }
}

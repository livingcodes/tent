using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Tent.Data
{
    public interface IReaderConverter<T>
    {
        T Convert(IDataReader reader);
    }

    public class ReaderToDataTable : IReaderConverter<DataTable>
    {
        public DataTable Convert(IDataReader reader) {
            var table = new DataTable();
            table.Load(reader);
            return table;
        }
    }

    public class ReaderToClassList<T> : IReaderConverter<List<T>>
    {
        public List<T> Convert(IDataReader reader) {
            var list = new List<T>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var columns = new GetColumns().From(reader);
            while (reader.Read()) {
                var item = System.Activator.CreateInstance<T>();
                foreach (var property in properties) {
                    if (columns.Contains(property.Name))
                        property.SetValue(item, reader[property.Name]);
                }
                list.Add(item);
            }
            return list;
        }
    }

    public class ReaderToStructList<T> : IReaderConverter<List<T>>
    {
        public List<T> Convert(IDataReader reader) {
            var list = new List<T>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var columns = new GetColumns().From(reader);
            while (reader.Read()) {
                var item = default(T);
                object obj = item; // box
                foreach (var property in properties) {
                    if (columns.Contains(property.Name))
                        property.SetValue(obj, reader[property.Name]);
                }
                item = (T)obj; // unbox
                list.Add(item);
            }
            return list;
        }
    }

    public class ReaderToValueList<T> : IReaderConverter<List<T>>
    {
        public List<T> Convert(IDataReader reader) {
            var list = new List<T>();
            while (reader.Read()) {
                list.Add((T)reader[0]);
            }
            return list;
        }
    }

    public class ReaderToList<T> : IReaderConverter<List<T>>
    {
        public List<T> Convert(IDataReader reader) {
            if (typeof(T) == typeof(string)
            ||  typeof(T) == typeof(int)
            ||  typeof(T) == typeof(DateTime)
            ||  typeof(T) == typeof(double)) // todo: decimal, long
                return new ReaderToValueList<T>().Convert(reader);
            else if (typeof(T).IsClass)
                return new ReaderToClassList<T>().Convert(reader);
            else
                return new ReaderToStructList<T>().Convert(reader);
        }
    }

    // if table contains column with property name
    //   then set property to column value
    // if table does not contain column with property name
    //   then ignore property
    public class ReaderToClass<T> : IReaderConverter<T>
    {
        public T Convert(IDataReader reader) {
            var item = default(T);
            var columns = new GetColumns().From(reader);
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            while (reader.Read()) {
                item = Activator.CreateInstance<T>();
                foreach (var property in properties) {
                    if (columns.Contains(property.Name))
                        property.SetValue(item, reader[property.Name]);
                }
                break;
            }
            return item;
        }
    }

    public class ReaderToValue<T> : IReaderConverter<T>
    {
        public T Convert(IDataReader reader) {
            var value = default(T);
            while (reader.Read()) {
                value = (T)reader[0];
                break;
            }
            return value;
        }
    }

    public class ReaderToStruct<T> : IReaderConverter<T>
    {
        public T Convert(IDataReader reader) {
            var item = default(T);
            object boxed = item;
            var columns = new GetColumns().From(reader);
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            while (reader.Read()) {
                foreach (var property in properties) {
                    if (columns.Contains(property.Name))
                        property.SetValue(boxed, reader[property.Name]);
                }
                item = (T)boxed;
                break;
            }
            return item;
        }
    }

    public class ReaderToItem<T> : IReaderConverter<T>
    {
        public T Convert(IDataReader reader) {
            if (typeof(T) == typeof(string)
            ||  typeof(T) == typeof(int)
            ||  typeof(T) == typeof(DateTime)
            ||  typeof(T) == typeof(double))
                return new ReaderToValue<T>().Convert(reader);
            if (typeof(T).IsClass)
                return new ReaderToClass<T>().Convert(reader);
            else
                return new ReaderToStruct<T>().Convert(reader);
        }
    }

    public class GetColumns
    {
        public List<string> From(IDataReader reader) {
            var columnNameList = new List<string>();
            var count = reader.FieldCount;
            for (var i = 0; i < count; i++) {
                var columnName = reader.GetName(i);
                columnNameList.Add(columnName);
            }
            return columnNameList;
        }

        public List<string> From(string tableName, IDatabase db) {
            // get from cache
            // var columnNameList = cache.Get<List<string>>($"ColumnsFor{tableName}");
            // if (columnNameList == null) {
                // get from database
                var columnNameList = db.Select<string>(
                    $@"SELECT COLUMN_NAME FROM Tent.INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = '{tableName}'"
                );
            // }
            return columnNameList;
        }
    }
}
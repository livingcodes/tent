using System;
using System.Collections.Generic;
using System.Data;

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

    public class ReaderToList<T> : IReaderConverter<List<T>>
    {
        public List<T> Convert(IDataReader reader) {
            var list = new List<T>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            while (reader.Read()) {
                var item = System.Activator.CreateInstance<T>();
                foreach (var property in properties) {
                    property.SetValue(item, reader[property.Name]);
                }
                list.Add(item);
            }
            return list;
        }
    }

    public class ReaderToClass<T> : IReaderConverter<T>
    {
        public T Convert(IDataReader reader) {
            var item = default(T);
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            while (reader.Read()) {
                item = Activator.CreateInstance<T>();
                foreach (var property in properties) {
                    property.SetValue(item, reader[property.Name]);
                }
                break;
            }
            return item;
        }
    }
}

using System;
using System.Data.SqlClient;

namespace Tent
{
    public interface ISqlBuilder
    {
        string BuildInsertSql();
        string BuildUpdateSql();
    }
    public class SqlBuilder<T> : ISqlBuilder
    {
        public SqlBuilder(T instance, SqlCommand command) {
            this.instance = instance;
            this.command = command;
        }
        T instance;
        SqlCommand command;

        public string BuildInsertSql() {
            var tableName = instance.GetType().Name + "s";
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var columnNames = "";
            var values = "";
            foreach (var property in properties) {
                if (property.Name.ToUpper() == "ID")
                    continue;

                var value = property.GetValue(instance);
                if (value is null)
                    value = DBNull.Value;
                columnNames += property.Name + ", ";

                command.Parameters.AddWithValue("@" + property.Name, value);
                values += "@" + property.Name + ", ";
            }
            columnNames = columnNames.Substring(0, columnNames.Length - 2);
            values = values.Substring(0, values.Length - 2);
            var sql = $@"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";
            return sql;
        }

        public string BuildUpdateSql() {
            var tableName = instance.GetType().Name + "s";
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var setters = "";
            var id = "0";
            foreach (var property in properties) {
                if (property.Name.ToUpper() == "ID") {
                    id = property.GetValue(instance).ToStringOr("0");
                    continue;
                }

                var value = property.GetValue(instance);
                if (value is null)
                    value = DBNull.Value;
                
                setters += $"{property.Name} = @{property.Name}, ";

                command.Parameters.AddWithValue("@" + property.Name, value);
            }
            setters = setters.Substring(0, setters.Length - 2);
            var sql = $@"update {tableName} set {setters} where id = {id}";
            return sql;
        }
    }
}
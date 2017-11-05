﻿using System.Collections.Generic;

namespace Tent.Data
{
    public class Pack : Backpack
    {
        public Pack() 
        : base(new SqlConnectionFactory(), new Reader()) 
        {}
    }

    public class Backpack
    {
        public Backpack(
            IConnectionFactory connectionFactory, 
            IRead reader
        ) {
            this.reader = reader;
            this.connectionFactory = connectionFactory;
        }
        string sql;
        IConnectionFactory connectionFactory;
        IRead reader;

        IQuery query { get {
            if (_query == null)
                _query = new Query(connectionFactory, reader);
            return _query;
        } }
        IQuery _query;

        void setQueryToNull() {
            _query = null;
        }

        public Backpack Sql(string sql) {
            query.Sql(sql);
            return this;
        }

        public Backpack Parameter(string name, object value) {
            query.Parameter(name, value);
            return this;
        }

        public List<T> Select<T>(string sql = null, params object[] parameters) {
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql(query.Sql());
            if (parameterNames.Count > 0) {
                if (parameters.Length > 0) {
                    if (parameters.Length != parameterNames.Count)
                        throw new System.Exception($"Parameter name and value counts are not equal. Parameter name count: {parameterNames.Count}, Parameter value count: {parameters.Length}");
                    for (var i = 0; i < parameters.Length; i++)
                        query.Parameter(parameterNames[i], parameters[i]);
                }
            }
            var list = query.Select<T>();
            setQueryToNull();
            return list;
        }

        public T SelectOne<T>(string sql = null, params object[] parameters) {
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql(query.Sql());
            if (parameterNames.Count > 0)
                for (var i = 0; i < parameters.Length; i++)
                    query.Parameter(parameterNames[i], parameters[i]);
            var item = query.SelectOne<T>();
            setQueryToNull();
            return item;
        }

        public T Select<T>(int id) {
            return SelectOne<T>($"select * from {typeof(T).Name}s where id = @id", id);
        }

        public int Execute(string sql = null, params object[] parameters) {
            if (sql != null)
                query.Sql(sql);
            var parameterNames = getParameterNamesFromSql(query.Sql());
            if (parameterNames.Count > 0)
                for (var i = 0; i < parameters.Length; i++)
                    query.Parameter(parameterNames[i], parameters[i]);
            int affectedRows = query.Execute(query.Sql());
            setQueryToNull();
            return affectedRows;
        }

        public void DropTable(string tableName) {
            Select<int>($"drop table {tableName}");
        }

        List<string> getParameterNamesFromSql(string sql) {
            var parameters = new Dictionary<string, int>();
            var index = sql.IndexOf('@');
            while (index > -1) {
                var endIndex = sql.IndexOfAny(new char[] {' ', ',', ')'}, index);
                if (endIndex == -1)
                    endIndex = sql.Length - 1;
                else
                    endIndex -= 1;
                
                var parameterName = sql.Substring(index, endIndex - index + 1);
                if (!parameters.ContainsKey(parameterName))
                    parameters.Add(parameterName, index);
                index = sql.IndexOf('@', index + 1);
            };
            var keys = new List<string>();
            foreach (var key in parameters.Keys)
                keys.Add(key);
            return keys;
        }
    }

    public class Admin
    {
        public Admin(Backpack db) {
            this.db = db;
        }
        Backpack db;
        public void DropTable(string tableName) {
            db.Select<int>($"drop table {tableName}");
        }
    }
}
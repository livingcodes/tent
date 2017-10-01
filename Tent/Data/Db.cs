using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace Tent.Data
{
    // Not implemented. Mocking potential api
    public class Db
    {
        public Db(string connectionString) {
            this.connectionString = connectionString;
        }
        string connectionString;

        public T Select<T>(int id) {
            return new Query(connectionString).SelectOne<T>("select * from " + typeof(T).Name + "s where id = " + id.ToString());
        }

        public IQuery Query(string sql, params object[] parameters) {
            return new Query(connectionString, sql, parameters);
        }
        public List<T> Select<T>(string sql, params object[] parameters) {
            return new Query(connectionString, sql, parameters).Select<T>();
        }
        public T SelectOne<T>(string sql, params object[] parameters) {
            return new Query(connectionString, sql, parameters).SelectOne<T>();
        }
        public DataTable SelectTable(string sql, params object[] parameters) {
            return new Query(connectionString, sql, parameters).SelectTable();
        }
        public IQuery Sproc(string name, params object[] parameters) {
            return new Query(connectionString, $"exec {name}", parameters);
        }
    }
    class DbExamples
    {
        public void Examples() {
            var db = new Db(connectionString: "");
            var sql = "";

            db.Select<Point>(1);
            db.Select<Point>("select * from Points");
            db.Select<Point>("select * from Points where published < @start", DateTime.Now);
            db.SelectOne<Point>("select top 1 * from Points order by id desc");
            db.SelectTable("select * from Points");
            db.Query("select * from Points where published < @start", DateTime.Now)
                .Select<Point>();
            db.Query("select * from Points")
                .Parameter("@Start", DateTime.Now)
                .Select<Point>();
            db.Query("select top 1 * from Points order by id desc")
                .SelectOne<Point>();
            db.Query("select * from Points")
                .SelectTable();

            var point = db.SelectOne<Point>(sql);

            var points = new Query("select * from Points where @Start > '2017-09-28'")
                .Parameter("@Start", DateTime.Now)
                .Select<Point>();
        }
    }
}

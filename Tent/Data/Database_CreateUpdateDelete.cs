namespace Tent.Data
{
    // partial class with create, update and delete methods
    // that use the basic database implementation
    public partial class Database
    {
        IDatabase simpleDb { get {
            if (_simpleDb == null)
                _simpleDb = new SimpleDatabase(connectionFactory, reader, tableName, cache);
            return _simpleDb;
        } }
        IDatabase _simpleDb;

        /// <summary>Insert object into datbase. Returns id.</summary>
        public int Insert<T>(T instance) =>
            simpleDb.Insert(instance);

        public int Update<T>(T instance) =>
            simpleDb.Update(instance);

        public int Delete<T>(int id) =>
            simpleDb.Delete<T>(id);
    }
}

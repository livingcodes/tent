namespace Tent
{
    // prototype
    public class Table
    {
        public void Example() {
            var posts = new Table("Posts")
                .AddColumn("Id", SqlType.Int, Syntax.Identity(1, 1))
                .AddColumn("Title", SqlType.VarChar(80), Syntax.NotNull)
                .AddColumn("Html", SqlType.VarCharMax)
                //.AddColumnDateCreated()
                //.AddColumnLastModified();
                .End();
            //posts.Sql;
        }

        public Table(string name) {
            Name = name;
            Sql = $@"
            IF EXISTS (
                SELECT * FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'Posts'
            )
                drop table {name}
            create table {name} (";
        }

        public string Name, Sql;

        public Table AddColumn(string columnName, SqlType sqlType, string syntax = "") {
            Sql += $"{columnName} {sqlType} {syntax},\r\n";
            return this;
        }

        public Table End() {
            var lastIndex = Sql.LastIndexOf(",\r\n");
            Sql = Sql.Remove(lastIndex, 3);
            Sql = Sql.Insert(lastIndex, "\r\n");
            Sql += ")";
            return this;
        }

        public class Syntax
        {
            public static string Identity(int start, int increment)
                => $" identity({start},{increment})";
            
            public static string NotNull => " not null";
        }

        public class SqlType
        {
            private SqlType(string name)
                => Name = name;
            
            public string Name { get; private set; }

            public static SqlType Int 
                = new SqlType("int");
            public static SqlType VarChar(int numberOfCharacters) 
                => new SqlType($"varchar({numberOfCharacters})");
            public static SqlType VarCharMax 
                = new SqlType("varchar(max)");

            public override string ToString() {
                return Name;
            }
        }
    }
}

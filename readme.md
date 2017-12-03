Tent is a lightweight content management system (CMS). It is just for fun and in early stages. The goal is to make a quick and easy way to create a new content type with data access for developer and edit screens for editors.

# Getting Started
* set connection string
```
public class ConnectionFactory : IConnectionFactory
{
	public IDbConnection Create() {
		return new SqlConnection(connectionString);
	}
}
```

# To Do
* Select value type DateTime (string and int work)
* Handle null datetime
* Configuration to insert or update properties and/or fields
* Map columns to properties with different names
* Cache class properties
* Return tuple var p = db.Select<Post, (string Title, string Html)>(post.Id);
* Return multiple classes from single query
* Log queries and exceptions
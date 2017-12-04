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
* Handle null datetime
* Cache class properties
* Configuration to insert or update properties and/or fields
* Map columns to properties with different names
* Start, commit and rollback transaction
* Share connection
* Return multiple classes from single query
* Async select
* Return tuple var p = db.Select<Post, (string Title, string Html)>(post.Id);
* Log queries and exceptions
* Log long running queries
* Paging
* Events ??? open connection event, close connection event, long running query event
* Support different data providers (like postgres, mysql, memory)
* Imagine: intellisense for inline sql (for syntax and table and column names)
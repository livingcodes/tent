Tent is a lightweight content management system (CMS). It is just for fun and in early stages. The goal is to make a quick and easy way to create a new content type with data access for developer and edit screens for editors.

# Data

Intuitive api to query data.

```
var user = db.Select<User>("select * from [user] where email = @email", email);
```

So what's going on here...probably exactly what you think.

### Automapping

It automatically maps the SQL result to a strongly typed object. 
You don't need to write any left-right code.
You don't even need any configuration to tell it how to map the columns to the properties.
It matches columns and properties by name. 
For example, a "LastName" database column maps to the "LastName" property of a class.

Now you might be thinking, my column names don't match my property names.
Yeah that's the downside to keeping the concepts simple. 
If you can't or choose not to have matching columns and properties.

### Standard SQL

Not much else to say...you query using standard, familiar SQL.
No need to contemplate how the api is going to interpret the SQL,
because, it doesn't modify the SQL. It passes the SQL directly to the database.
The shorter syntax is an exception to this, but it is optional so if you don't
want to think about it then just write the same SQL you always do.

### Safe, Simple Parameter Values

Use parameterized queries and then just pass the parameter values in the order they appear in the SQL.
This prevents SQL injection.
If you want to be more explicit about parameter names and values, the api supports that too.

What else can it do?

### Other CRUD

So we covered *read* but what about the other CRUD.
Yep can *create*, *update* and *delete* too.
Same as with reading it will map the properties to the columns for you.

```
db.Insert(user);
db.Update(user);
db.Delete(user);
db.Delete<User>(id);
```

A couple of optional uses, if you prefer...

### Short Syntax

It supports a shorter SQL syntax.

```
db.Select<User>("where email = @email", email);
```

It infers the select portion of the sql statement based on the generic type.
So no need to type `select * from [user]`.
A little bit simpler without losing any intention; 
the syntax reads like you would verbally discuss it, "Select user where email equals email."

### Explicit Parameter Name-Value Pairs

Don't want to worry about order of parameters...don't blame you.

```
db.Sql("where email = @email and token = @token")
   .Parameter("@token", token)
   .Parameter("@email", email)
   .Select<User>();
```

### Finally

The api is an attempt to keep data access simple, intuitive and fast with as few steps required as possible.
I find it makes the code especially easy to follow for simple queries.

There's some code in the project for the following to.

* api to create SQL tables
* cache queries, `db.Cache(key).SelectOne<User>(id);`
* only interested in one result, `db.SelectOne<User>("where id = @id");`
* if you're querying by id, it's even more simple, `db.SelectOne<User>(id);`
* like sprocs, `db.Sproc("GetUser").Parameter("@Id", id).Select<User>();`

# Getting Started

Set connection string

```
public class ConnectionFactory : IConnectionFactory
{
	public IDbConnection Create() {
		return new SqlConnection(connectionString);
	}
}
```

# To Do
* Paging
* Handle nullable value types datetime
* Cache class properties
* Configuration to insert or update properties and/or fields
* Map columns to properties with different names
* Start, commit and rollback transaction
* Share connection
* Return multiple classes from single query
* Async select
* Return tuple var p = db.Select<Post, (string Title, string Html)>(post.Id); looks like it is not possible :(
* Log queries and exceptions
* Log long running queries
* Events ??? open connection event, close connection event, long running query event
* Support different data providers (like postgres, mysql, memory)
* Imagine: intellisense for inline sql (for syntax and table and column names)
* Admin: db.Truncate<T>(); db.CreateProcedure("GetStuff", "SELECT * FROM Stuff");
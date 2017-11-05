# Getting Started
* set connection string

The connection string file path is set ...

# To Do
* Configuration to insert or update properties and/or fields
X Property does not have column
X During queries ignore properties that don't have matching column
* Map columns to properties with different names
* Select value type (string, int, DateTime)
X Sproc
X Move connection string from appsettings.json to someplace out of source control
X Get DataTable
* Cache class properties
* Cache return result
* Handle null datetime
* Return tuple var p = db.Select<Post, (string Title, string Html)>(post.Id);
* Return multiple classes from single query
* Log queries and exceptions
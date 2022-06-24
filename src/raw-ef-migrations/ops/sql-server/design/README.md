# SQL Server design time project.

This project has the intent of managed the design of SQL Server with [dotnet-ef](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Requirements.

Create a file named settings.json on project directory like as
``` json
{
  "ConnectionStrings": {
    "SqlServerDesignTime": "<YOUR_CONNECTION_STRING_HERE>"
  }
}
```

## Raw migrations.

Sometimes we would use complex queries on migrations or seeding operations into tables with dummy data.

A useful way to execute the `SQL` script could be crate a script file with the same name of target migration, like as `20201222093352_FooBar`, and "link" that into the migration class with something like this:

``` cs
var migrationAttribute = (MigrationAttribute)this.GetType()
    .GetCustomAttributes(typeof(MigrationAttribute), false)
    .Single();

migrationBuilder.Sql(File.ReadAllText(string.Format(
    CultureInfo.InvariantCulture,
    "{1}{0}RawMigrations{0}{2}",
    Path.DirectorySeparatorChar,
    AppContext.BaseDirectory,
    $"{migrationAttribute.Id}.sql")));
```

## Database Commands

1. `dotnet ef database drop`: Drops the current database, deleting all data and tables. Use this command with caution.

2. `dotnet ef migrations add <MigrationName>`: Adds a new migration with the name `<MigrationName>`. This allows you to update your database schema to match your codebase while preserving existing data.

3. `dotnet ef database update`: Applies any pending migrations to update the database schema.

## Migrator

To use the migrator, you need to open a terminal to the main solution folder.
Then run the following command.

### The command
`dotnet ef migrations add YourMigration -s Timely.Migrator -p Timely.Persistence -c Timely.Persistence.TimelyContext`

### Acknowledgement
Thanks to the person who wrote this article.
https://medium.com/@taublast/entity-framework-with-code-first-migrations-in-net-maui-3efbdb765592
namespace Timely.Persistence;

using Microsoft.EntityFrameworkCore;

using Timely.Domain.Features.TodoItems;

public class TimelyContext : DbContext
{
    #region Static Propeties

    /// <summary>
    /// The file path to the database.
    /// </summary>
    public static string FilePath { get; protected set; } = "";

    /// <summary>
    /// A flag that indicates if the service has already been initialized.
    /// This is necessary because the service is transient, but only needs
    /// to be run once on startup.
    /// </summary>
    public static bool Initialized { get; protected set; }

    #endregion

    #region DbSets
    public DbSet<TodoItem> TodoItems { get; set; }

    #endregion

    #region Constructors
    /// <summary>
    /// The constructor used when creating migrations with the Migrator project.
    /// </summary>
    public TimelyContext()
    {
        FilePath = Path.Combine("../", "timelyMigrator.db3");
        Initialize();
    }

    /// <summary>
    /// The constructor used to create a new context for the application to run.
    /// </summary>
    /// <param name="filenameWithPath">The filename and full path for the SQLite database.</param>
    public TimelyContext(string filenameWithPath)
    {
        FilePath = filenameWithPath;
        Initialize();
    }
    #endregion

    #region Configuration
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Filename={FilePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimelyContext).Assembly);

    private void Initialize()
    {
        if (!Initialized)
        {
            Initialized = true;

            SQLitePCL.Batteries_V2.Init();

            Database.Migrate();
        }
    }

    public void Reload()
    {
        Database.CloseConnection();
        Database.OpenConnection();
    }

    #endregion
}

using System.Data.Common;
using System.Data.Entity;
using Abp.EntityFramework;
using WebFileSystem.Entities.EF;

namespace WebFileSystem.EntityFramework
{
    public class WebFileSystemDbContext : AbpDbContext
    {
        public virtual IDbSet<File> Files { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public WebFileSystemDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in WebFileSystemDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of WebFileSystemDbContext since ABP automatically handles it.
         */
        public WebFileSystemDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public WebFileSystemDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public WebFileSystemDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);
            base.OnModelCreating(modelBuilder);
        }

        private static string defaultSchema;
        public static string DefaultSchema {
            get {
                if (string.IsNullOrEmpty(defaultSchema))
                {
                    defaultSchema = System.Configuration.ConfigurationManager.AppSettings["DefaultSchema"];
                }
                return defaultSchema;
            }
        }
    }
}

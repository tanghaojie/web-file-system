using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using WebFileSystem.EntityFramework;

namespace WebFileSystem
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(WebFileSystemCoreModule))]
    public class WebFileSystemDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<WebFileSystemDbContext>(null);
        }
    }
}

using System.Reflection;
using Abp.Modules;
using Abp.Configuration.Startup;

namespace WebFileSystem
{
    public class WebFileSystemCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.MultiTenancy.IsEnabled = false;
            Configuration.Auditing.IsEnabled = false;
            Configuration.Localization.IsEnabled = false;
            Configuration.Navigation.Providers.Clear();
            //Configuration.Settings.Providers.Add<AppSettingProvider>();
            Configuration.ReplaceService<Abp.MultiTenancy.ITenantStore, Tenant.SingleTenantStore>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

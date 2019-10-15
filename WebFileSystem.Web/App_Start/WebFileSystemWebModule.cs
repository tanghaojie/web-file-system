using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Web.Mvc;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebFileSystem.Web {
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(WebFileSystemDataModule), 
        typeof(WebFileSystemApplicationModule), 
        typeof(WebFileSystemWebApiModule))]
    public class WebFileSystemWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}

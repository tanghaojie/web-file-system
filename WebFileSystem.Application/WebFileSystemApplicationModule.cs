using System.Reflection;
using Abp.Modules;

namespace WebFileSystem
{
    [DependsOn(typeof(WebFileSystemCoreModule))]
    public class WebFileSystemApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

using Abp.Application.Services;

namespace WebFileSystem
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class WebFileSystemAppServiceBase : ApplicationService
    {
        protected WebFileSystemAppServiceBase()
        {
            LocalizationSourceName = WebFileSystemConsts.LocalizationSourceName;
        }
    }
}
using Abp.Web.Mvc.Controllers;

namespace WebFileSystem.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class WebFileSystemControllerBase : AbpController
    {
        protected WebFileSystemControllerBase()
        {
            LocalizationSourceName = WebFileSystemConsts.LocalizationSourceName;
        }
    }
}
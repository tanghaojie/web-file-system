using Abp.Web.Mvc.Views;

namespace WebFileSystem.Web.Views
{
    public abstract class WebFileSystemWebViewPageBase : WebFileSystemWebViewPageBase<dynamic>
    {

    }

    public abstract class WebFileSystemWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected WebFileSystemWebViewPageBase()
        {
            LocalizationSourceName = WebFileSystemConsts.LocalizationSourceName;
        }
    }
}
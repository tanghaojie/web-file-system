using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Swashbuckle.Application;
using WebFileSystem.Swagger;

namespace WebFileSystem {
    [DependsOn(typeof(AbpWebApiModule), typeof(WebFileSystemApplicationModule))]
    public class WebFileSystemWebApiModule : AbpModule {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
           
            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(WebFileSystemApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.MapHttpAttributeRoutes();

            var rootUrl = System.Configuration.ConfigurationManager.AppSettings["UseSwaggerRoot"];
            if (!string.IsNullOrEmpty(rootUrl))
            {
                ConfigureSwaggerUi(rootUrl);
            }
        }
        private void ConfigureSwaggerUi(string root)
        {
            var baseDirectory = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");

            var commentsFileName = "WebFileSystem.Application.XML";
            var commentsFile = Path.Combine(baseDirectory, commentsFileName);

            var commentsFileName2 = "WebFileSystem.WebApi.XML";
            var commentsFile2 = Path.Combine(baseDirectory, commentsFileName2);

            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "接口文档");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    c.IncludeXmlComments(commentsFile);
                    c.IncludeXmlComments(commentsFile2);
                    c.OperationFilter<SwaggerFormDataFilter>();
#if !DEBUG
                    c.RootUrl(req => root);
#endif
                })
                .EnableSwaggerUi(c =>
                {
                    c.InjectJavaScript(Assembly.GetAssembly(typeof(WebFileSystemWebApiModule)), "WebFileSystem.Swagger.Swagger.js");
                });
        }
    }
}

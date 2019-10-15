using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace WebFileSystem.Swagger {
    public class SwaggerFormDataFilter : IOperationFilter {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var method = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerFormDataMethodAttribute>().FirstOrDefault();
            if (method != null)
            {
                operation.consumes.Clear();
                operation.consumes.Add("multipart/form-data");
                if (operation.parameters == null) { operation.parameters = new List<Parameter>(); }
                operation.parameters.Clear();

                var len1 = method.RequiredList?.Length ?? 0;
                var len2 = method.NameList?.Length ?? 0;
                var len3 = method.DescriptionList?.Length ?? 0;
                var len4 = method.TypeList?.Length ?? 0;
                var len = len1;
                if (len2 > len) { len = len2; }
                if (len3 > len) { len = len3; }
                if (len4 > len) { len = len4; }

                for (int i = 0; i < len; i++)
                {
                    var parameter = new Parameter
                    {
                        name = "name",
                        @in = "formData",
                        description = "des",
                        required = false,
                        type = "file"
                    };
                    if (i < len1) { parameter.required = method.RequiredList[i]; }
                    if (i < len2) { parameter.name = method.NameList[i] ?? ""; }
                    if (i < len3) { parameter.description = method.DescriptionList[i] ?? ""; }
                    if (i < len4) { parameter.type = method.TypeList[i].ToString().ToLower(); }

                    operation.parameters.Add(parameter);
                }
            }
        }
    }
}

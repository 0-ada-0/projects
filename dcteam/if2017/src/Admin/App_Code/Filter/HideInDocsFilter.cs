using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HideInDocsAttribute : Attribute
    {
    }

    public class HideInDocsFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptionsGroups.Items.SelectMany(e => e.Items))
            {
                if (apiDescription.GetControllerAttributes().OfType<HideInDocsAttribute>().Count() == 0 && apiDescription.GetActionAttributes().OfType<HideInDocsAttribute>().Count() == 0) continue;

                var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                if (!key.Contains("/test/") && swaggerDoc.Paths.ContainsKey(key))
                {
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }
    }
}

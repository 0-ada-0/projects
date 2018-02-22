using IF2017.Admin.Filter;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Extensions
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware(typeof(RequestLoggerMiddleware));
        }
    }
}

using BackendServiceDispatcher.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendServiceDispatcher.Extensions
{
    public static class AntiforgeryForAngularJsMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiforgeryForAngularJs(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SendAntiforgeryForAngularJsMiddleware>();
        }
    }
}

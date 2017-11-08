using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendServiceDispatcher.Middlewares
{
    public class SendAntiforgeryForAngularJsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public SendAntiforgeryForAngularJsMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public Task Invoke(HttpContext context)
        {
            string requestedPath = context.Request.Path.Value;
            if (requestedPath != null && requestedPath.ToLower().Contains("/api"))
            {
                var tokens = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append
                    (
                        "XSRF-TOKEN", 
                        tokens.RequestToken, 
                        new CookieOptions()
                        {
                            HttpOnly = false,
                            Secure = true
                        }
                    );
            }
            return _next(context);
        }
    }
}

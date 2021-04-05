using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XChange.Api.Middlewares;

namespace XChange.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void ConfigureBuiltinExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    options => {
                        options.Run(
                            async context => {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                var ex = context.Features.Get<IExceptionHandlerFeature>();

                                if (ex != null)
                                {
                                    await context.Response.WriteAsync(ex.Error.Message);
                                }
                            }
                        );
                    }
                );
            }


        }
    }
}

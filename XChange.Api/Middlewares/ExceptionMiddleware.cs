using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XChange.Api.DTO;

namespace XChange.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostingEnvironment env;
        public ExceptionMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            this.env = env;
            this.next = next;

        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {

                ApiError response;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                String message;
                var exceptionType = ex.GetType();

                if(exceptionType == typeof(UnauthorizedAccessException))
                {
                    statusCode = HttpStatusCode.Forbidden;
                    message = "You are not authorized";
                }
               
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Some unknown error occured";
                }

                if (env.IsDevelopment())
                {
                    response = new ApiError((int)statusCode, ex.Message, ex.StackTrace.ToString());
                } else
                {
                    response = new ApiError((int)statusCode, message);

                }

                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}

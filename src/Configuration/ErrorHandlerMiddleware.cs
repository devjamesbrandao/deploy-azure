
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MeuTodo.Configuration
{
    public static class ErrorHandlerMiddleware
    {
        public class GlobalExceptionHandlerMiddleware : IMiddleware
        {
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private static Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return context.Response.WriteAsync(JsonConvert.SerializeObject("Internal Server erro [500]"));
            }
        }
    }
}
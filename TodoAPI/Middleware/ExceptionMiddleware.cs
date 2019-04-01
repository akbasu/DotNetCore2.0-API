using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using TodoAPI.Common;
using System.Net;

namespace TodoAPI.Middleware
{
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as CustomException;
            var httpStatusCode = HttpStatusCode.InternalServerError;
            var message = "Unexpected server error";
            var description = "Unexpected server error";

            if (customException != null)
            {
                message = customException.Message;
                description = customException.Description;
                httpStatusCode = customException.StatusCode;
            }

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;
            
            await response.WriteAsync(
                JsonConvert.SerializeObject(
                    new CustomError
                    {
                        Message = message,
                        Description = description
                    }
                )
            );
        }
    }
}

﻿using Newtonsoft.Json;
using System.Net;

namespace CompanyBlazor5.Server
{
    public class ErrorHandlingMiddleware
    {
        readonly RequestDelegate _next;
        static ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        static async Task HandleExceptionAsync(
             HttpContext context,
             Exception exception)
        {
            string error = "An internal server error has occured.";

            _logger.LogError($"{exception.Source} - {exception.Message} - {exception.StackTrace} - {exception.TargetSite.Name}");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error
            }));
        }
    }
}

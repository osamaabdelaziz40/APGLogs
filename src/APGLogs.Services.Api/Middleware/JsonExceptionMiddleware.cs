using APGLogs.Constant;
using APGLogs.Domain.Interfaces;
using APGLogs.Domain.Models;
using APGLogs.DomainHelper.Exceptions;
using APGLogs.DomainHelper.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace APGLogs.Services.Api.Middleware
{
    public class JsonExceptionMiddleware
    {
        public async Task Invoke(HttpContext httpContext)
        {
            // Save Exception to DB 
            // Handle Business Exception --> return same message
            // Bad Request --> return status code 400 - message 'E-Retail Exception + Exception Id'
            // Default --> return Message 'E-Retail Exception + Exception Id'

            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception == null)
                return;


            string data = null;
            //if (exception.Data.Values.Any())
            //    data = string.Join(",", exception.Data.Values);
            string innerExeption = exception.InnerException != null ? exception.InnerException.Message : null;
            try
            {
                //TODO: Instead of calling DB we have to call APG.Log\LogException we are testing the exception log here only
                var exceptionLogRepository = httpContext.RequestServices.GetService<IExceptionLogRepository>();
                var exceptionLog = new ExceptionLog();// exception.Message, exception.Source, exception.StackTrace, innerExeption, data, DateTime.Now, string.Empty, null);
                exceptionLog.Id = Guid.NewGuid().ToString();
                exceptionLog.Message = exception.Message;
                exceptionLog.Source = exception.Source;
                exceptionLog.StackTrace = exception.StackTrace;
                exceptionLog.InnerException = innerExeption;
                exceptionLog.Data = data;
                exceptionLog.DateTime = DateTime.Now;
                exceptionLog.CommunicationLogId = null;
                exceptionLog.ExceptionType = CommonCostant.APGException;

                await exceptionLogRepository.Add(exceptionLog);

                httpContext.Items["ExceptionErrorId"] = exceptionLog.Id;
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (exception is BusinessException)
                {
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponse { Message = exception.Message })).ConfigureAwait(false);
                }
                else if (exception is BadRequestException)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponse { Message = $"APG Request Error: {exceptionLog.Id}" })).ConfigureAwait(false);
                }
                else
                {
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ExceptionResponse { Message = $"APG Request Error: {exceptionLog.Id}" })).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}

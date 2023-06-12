using System.Net;

namespace NZWalksAPI.Middlewares
{
    public class ExcptionHandlerMiddleware
    {
        private readonly ILogger<ExcptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExcptionHandlerMiddleware(ILogger<ExcptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext) 
        { 
            try
            {
                await next(httpContext);
            }
            catch(Exception ex) 
            {
                var errorID = Guid.NewGuid();
                // log this exception
                logger.LogError(ex, $"{errorID} : {ex.Message}" );

                // Return a custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorID,
                    ErrorMessage = ex.Message,
                    // "Something went wrong! We are looking into resolving this"  (when we are going to deploy use this)
                };
                // we don't have to convert  the error into Json Objext we havr a method that automatically does that for us
                await httpContext.Response.WriteAsJsonAsync(error);

            }

        }
    }
}

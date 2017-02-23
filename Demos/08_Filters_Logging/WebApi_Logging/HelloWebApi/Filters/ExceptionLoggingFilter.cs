using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HelloWebApi.Filters
{
    public class ExceptionLoggingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionLoggingFilter> _logger;

        public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger)
        {
            _logger = logger;
        }

        // NOTE: Always override OnExceptionAsync if performing I/O!

        public override void OnException(ExceptionContext context)
        {
            // Log user name: context.HttpContext.User.Identity.Name
            _logger.LogError($"Exception: {context.Exception.Message}",
                context.Exception);
        }
    }
}

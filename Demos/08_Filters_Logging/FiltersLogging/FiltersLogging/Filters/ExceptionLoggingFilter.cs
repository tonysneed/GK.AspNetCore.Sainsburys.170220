using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FiltersLogging.Filters
{
    public class ExceptionLoggingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionLoggingFilter> _logger;

        public ExceptionLoggingFilter(ILogger<ExceptionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError($"Exception: {context.Exception.Message}", context.Exception);
        }
    }
}

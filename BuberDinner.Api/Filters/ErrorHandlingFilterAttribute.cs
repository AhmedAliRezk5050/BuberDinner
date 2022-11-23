using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuberDinner.Api.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            context.Result = new ObjectResult(null)
            {
                StatusCode = 500,
                Value = exception.Message
            };
            context.ExceptionHandled = true;
        }
    }
}

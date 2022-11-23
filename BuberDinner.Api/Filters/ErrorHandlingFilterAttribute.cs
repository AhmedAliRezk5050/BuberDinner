using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BuberDinner.Api.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var problemDetails = new ProblemDetails()
            {
                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
                Title = "An Error occured while processing your request.",
                Status = (int)HttpStatusCode.InternalServerError
            };
            context.Result = new ObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            var (statusCode, message) = exception switch
            {
                DuplicateWaitObjectException => (StatusCodes.Status409Conflict, "Email already exists."),
                _ => (StatusCodes.Status400BadRequest, "An unexpected error occurred.")
            };

            return Problem(statusCode: statusCode, title: message);
            //return Problem(); => 500
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Application.BaseClass;

namespace IED.VTVMS.Host.Middleware
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                context.Result = new BadRequestObjectResult(new ErrorResponse(
                    "VALIDATION_ERROR",
                    string.Join("; ", errors.SelectMany(x => x.Value))
                ));
            }
        }
    }
}

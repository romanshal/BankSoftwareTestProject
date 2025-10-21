using BankSoftware.Application.Models;
using BankSoftware.Domain.Constants.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BankSoftware.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult MatchResponse(this ControllerBase controller, Result result)
        {
            if (result.IsFailure)
            {
                HandleError(result.Error, controller);
            }

            return controller.Ok();
        }

        public static ActionResult<TValue> MatchResponse<TValue>(this ControllerBase controller, Result<TValue> result)
        {
            if (result.IsFailure)
            {
                HandleError(result.Error, controller);
            }

            return controller.Ok(result.Value);
        }

        private static IActionResult HandleError(Error error, ControllerBase controller)
        {
            return error.Code switch
            {
                ErrorCodes.NotFound => controller.NotFound(error.Description),
                ErrorCodes.NullValue or ErrorCodes.CantCreate or ErrorCodes.CantUpdate => controller.BadRequest(error.Description),
                _ => controller.BadRequest(error.Description)
            };
        }
    }
}

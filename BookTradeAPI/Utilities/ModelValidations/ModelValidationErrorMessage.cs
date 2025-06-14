using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookTradeAPI.Utilities.ModelValidations
{
    public static class ModelValidationErrorMessage
    {
        public static List<string> GetErrorMessage(ModelStateDictionary modelState)
        {
            var errors = modelState
                    .Where(x => x.Value!.Errors.Any())
                    .SelectMany(x => x.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
            return errors;
        }
    }
}

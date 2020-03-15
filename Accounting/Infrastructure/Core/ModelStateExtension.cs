using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Accounting.Infrastructure.Core
{
    public static class ModelStateExtension
    {
        public static string ToErrorMessages(this ModelStateDictionary modelState)
        {
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    string errorMessage = "";
                    if (errors.Count == 1)
                    {
                        errorMessage = errors[0].ErrorMessage;
                        return errorMessage;
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = errors[i].ErrorMessage;
                        }

                        return string.Join(" ", errorMessages);
                    }
                }
            }
            return "";
        }
    }
}

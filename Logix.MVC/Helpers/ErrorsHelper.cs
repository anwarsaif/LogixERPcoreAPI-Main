using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Logix.MVC.Helpers
{
    public class ErrorsHelper
    {
        private readonly ModelStateDictionary modelState;

        public ErrorsHelper(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        public bool HasError(string property)
        {
            if (this.modelState.TryGetValue(property, out var propertyState)
            && propertyState.ValidationState == ModelValidationState.Invalid)
            {
                return true;
            }
            return false;
        }
        
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VkIntern.Filters.ActionFilters
{
    public class ValidateModel: ActionFilterAttribute
    {
        private string _modelName;
        public ValidateModel(string modelName = "viewmodel")
        {
            _modelName = modelName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(new
                {
                    error = context.ModelState.Values.Select(v => v.Errors),
                    model = context.ActionArguments.Select(arg => arg).Where(arg => arg.Key.ToLower().Contains(_modelName.ToLower())).Select(arg => arg.Value)
                });
        }
    }
}

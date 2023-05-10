﻿using DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using static VkIntern.Filters.Functions;
namespace VkIntern.Filters.ActionFilters
{
    public class ExistAttribute<T> : Attribute, IActionFilter where T : class
    {
        private readonly string _pathToId;

        public ExistAttribute(string pathToId = "id")
        {
            _pathToId = pathToId;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = (AppDbContext)context.HttpContext.RequestServices.GetRequiredService(typeof(AppDbContext));
            var id = IdFrom(_pathToId, context.ActionArguments);
            var entity = dbContext.Set<T>().Find(int.TryParse(id, out var intIdResult) ? intIdResult : id);
            if (entity == null)
                context.Result = new NotFoundObjectResult($"{typeof(T).Name} с id {id} не найден");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTaskManageApplication.Filter
{
    public class SampleGlobalActionFilter : IActionFilter,IOrderedFilter
    {
        private int _order =1;
        public SampleGlobalActionFilter()
        {

        }
        public SampleGlobalActionFilter(int Order )
        {
            _order = Order;
        }
        public int Order => _order;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.WriteAsync("GlobalActionFilter OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Response.WriteAsync("GlobalActionFilter OnActionExecuting");
        }
    }
}

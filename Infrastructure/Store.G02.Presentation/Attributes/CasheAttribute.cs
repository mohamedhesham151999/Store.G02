using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Store.G02.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation.Attributes
{
    public class CasheAttribute(int sec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var cashService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CasheService;

            var cashKey = GetCashKey(context.HttpContext.Request);

            var result = await cashService.GetAsync(cashKey);
            
            if(!string.IsNullOrEmpty(result))
            {
                var response = new ContentResult()
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = response;
                return;
            }

           var actionContext =  await next.Invoke();
           if(actionContext.Result is OkObjectResult okObjectResult) 
           {
              await cashService.setAsync(cashKey, okObjectResult.Value, TimeSpan.FromSeconds(sec));
           }
        }



        private string GetCashKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);

            foreach (var item in request.Query)
            {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString(); 
        }

    }
}

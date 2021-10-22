using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PreventDoublePostAttribute:ActionFilterAttribute
    {
        private const string UniqFormuId = "LastProcessedToken";
        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            IAntiforgery antiforgery = (IAntiforgery)context.HttpContext.RequestServices.GetService(typeof(IAntiforgery));
            AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context.HttpContext);
            if (!context.HttpContext.Request.Form.ContainsKey(tokens.FormFieldName))
                return;
            var currentFormId = context.HttpContext.Request.Form[tokens.FormFieldName].ToString();
            var lastToken = "" + context.HttpContext.Session.GetString(UniqFormuId);

            if (lastToken.Equals(currentFormId))
            {
                context.ModelState.AddModelError(string.Empty, $"{DateTime.Now.ToShortTimeString()}--重复提交");
                return;
            }
            context.HttpContext.Session.Remove(UniqFormuId);
            context.HttpContext.Session.SetString(UniqFormuId, currentFormId);
            await context.HttpContext.Session.CommitAsync();
            
        }
    }
}

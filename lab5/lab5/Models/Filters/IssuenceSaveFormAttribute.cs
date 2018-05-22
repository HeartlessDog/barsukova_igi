using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab5.Models.Filters
{
    public class IssuenceSaveFormAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var reader = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "ReaderID").Value.ToString();
            var date = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "DateOfIssuance").Value.ToString();
            var book = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "BookID").Value.ToString();
            var ret = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "Return").Value.ToString();

            var issuenceString = JsonConvert.SerializeObject(new Issuence() { ReaderID = Int32.Parse(reader), BookID = Int32.Parse(book), Return=Boolean.Parse(ret), DateOfIssuance = DateTime.Parse(date) });
            context.HttpContext.Session.SetString("Issuence", issuenceString);
        }
    }
}

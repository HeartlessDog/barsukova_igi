using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab5.Models.Filters
{
    public class ReaderSaveFormAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var name = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "FullName").Value.ToString();
            var date = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "DateOfBirth").Value.ToString();

            var readerString = JsonConvert.SerializeObject(new Reader() { FullName = name, DateOfBirth = DateTime.Parse(date) });
            context.HttpContext.Session.SetString("Reader", readerString);
        }
    }
}

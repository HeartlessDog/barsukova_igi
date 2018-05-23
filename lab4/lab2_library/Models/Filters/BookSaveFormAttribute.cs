using libr.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2_library.Models.Filters
{
    public class BookSaveFormAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var name = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "Name").Value.ToString();
            var year = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "YearOfEdition").Value.ToString();
            var jenre = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "Jenre").Value.ToString();
            var author = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "Author").Value.ToString();
            var edition = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "Edition").Value.ToString();
            var price = context.HttpContext.Request.Form.FirstOrDefault(p => p.Key == "Price").Value.ToString();

            var bookString = JsonConvert.SerializeObject(new Book() { Name = name, YearOfEdition = Int32.Parse(year),JenreID= Int32.Parse(jenre), Author=author, Edition=edition, Price = Int32.Parse(price)});
            context.HttpContext.Session.SetString("Book", bookString);
        }
    }
}

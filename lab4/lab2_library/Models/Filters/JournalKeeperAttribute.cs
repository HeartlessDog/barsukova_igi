using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2_library.Models.Filters
{
    public class JournalKeeperAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var serializedJournal = context.HttpContext.Session.GetString("Journal");
            var journal = serializedJournal == null ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(serializedJournal);
            var path = context.HttpContext.Request.Path.Value;
            if (path == "/")
            {
                journal.Add("User visited: /Home/Index");
            }
            else
            {
                if (new List<string>() { "/Books", "/Readers", "/Issuences" }.Contains(path))
                {
                    journal.Add("User visited: " + path + "/Index");
                }
                else
                {
                    journal.Add("User visited: " + path);
                }
            }
            serializedJournal = JsonConvert.SerializeObject(journal);
            context.HttpContext.Session.SetString("Journal", serializedJournal);
        }
    }
}

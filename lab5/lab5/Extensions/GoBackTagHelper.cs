using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace lab5.Extensions
{
    public class GoBackTagHelper: TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            context.AllAttributes.TryGetAttribute("link", out TagHelperAttribute tag);
            output.Attributes.SetAttribute("href", tag.Value);
            output.Content.SetContent("Go back");
        }
    }
}

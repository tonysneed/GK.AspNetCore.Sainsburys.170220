using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HelloTagHelpers.TagHelpers
{
    public class SpanTagHelper : TagHelper
    {
        public override void Process(
            TagHelperContext context, TagHelperOutput output)
        {
            var emoji = context.AllAttributes["emoji"];
            if (emoji != null && "smile" == emoji.Value.ToString())
            {
                output.Attributes.Add("title", "smile");
                output.Content.SetContent(" :) ");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }

}

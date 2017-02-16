using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HelloTagHelpers.TagHelpers
{
    // Specify custom element
    [HtmlTargetElement("emoji", Attributes = "face")]
    public class EmojiTagHelper : TagHelper
    {
        // Specify custom attribute
        [HtmlAttributeName("face")]
        public string Emoji { get; set; }

        public override void Process(
            TagHelperContext context, TagHelperOutput output)
        {
            if ("smile" == Emoji)
            {
                output.Attributes.Add("title", "smile");
                output.Content.SetContent(" :) ");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }

}

# HelloTagHelpers ReadMe

1. Add VS support for tag helpers

    - Add to "tools" section of project.json:

    ```
    "Microsoft.AspNetCore.Razor.Tools": "1.1.0-preview4-final"
    ```

    - Add dependency:

    ```json
    "Microsoft.AspNetCore.Razor.Tools": {
      "version": "1.1.0-preview4-final",
      "type": "build"
    }
    ```

    - Run `dotnet restore`

2. Add classes to TagHelpers folder

    - SpanTagHelper:

    ```csharp
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
    ```

    - EmojiTagHelper:

    ```csharp
    [HtmlTargetElement("emoji", Attributes = "face")]
    public class EmojiTagHelper : TagHelper
    {
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
    ```

3. Register custom tag helpers in Index.cshtml

    ```
    @addTagHelper "*, HelloTagHelpers"
    ```

4. Use tag helpers in Index.cshtml

    ```html
    <span emoji="smile"></span>
    <emoji face="smile"></emoji>
    ```

    - Each tag helper should display smile characters

5. Add package for MVC tag helpers in project.json

    ```
    "Microsoft.AspNetCore.Mvc.TagHelpers": "1.1.1"
    ```

6. Register MVC tag helpers in Index.cshtml

    ```
    @addTagHelper "*, Microsoft.AspNetCore.Mvc.TagHelpers"
    ```

    - Add a SayGoodBye action to HomeController.
    - Add an anchor tag using an asp-controller tag helper

    ```html
    <a asp-controller="Home" asp-action="SayGoodbye">Say Goodbye</a>
    ```


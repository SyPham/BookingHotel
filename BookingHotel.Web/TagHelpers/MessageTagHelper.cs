using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingHotel.Web.TagHelpers
{
    public class MessageTagHelper : TagHelper
    {
        public enum MessageType
        {
            info,
            success,
            danger,
            warning
        }

        public MessageType? Type { get; set; } = MessageType.info;

        public string Content { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Lấy các giá trị được cấu hình trong thuộc tính
            string type = this.Type.ToString().ToLower();
            string content = this.Content;

            var elem = await output.GetChildContentAsync();
            content += elem.GetContent();
            this.Content = content;

            string template = $@"
                        <div class='alert alert-{type} alert-dismissible fade show' role='alert'>
                            {content}
                            <button type='button' class='close' data-dismiss='alert' aria-label='Close'>
                                <span aria-hidden='true'>&times;</span>
                            </button>
                        </div>";

            output.TagName = string.Empty;

            if (content != string.Empty)
                output.Content.SetHtmlContent(template);
            else
                output.Content.SetHtmlContent(string.Empty);
        }
    }
}

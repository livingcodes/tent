using Microsoft.AspNetCore.Mvc;
using System;

namespace Tent.Pages.blog
{
    public class PostPage : BasePage
    {
        public void OnGet() {
            Post = new PostModel {
                Title = "Simple Data Access",
                Body = "Check it out...",
                PublishDate = DateTime.Now
            };
        }

        public PostModel Post { get; set; }
    }

    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
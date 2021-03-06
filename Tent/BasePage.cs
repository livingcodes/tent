﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tent
{
    public class BasePage : PageModel
    {
        /// <summary>Title shown in browser</summary>
        public string Title {
            get { return ViewData["Title"].ToStringOr(""); }
            set { ViewData["Title"] = value; }
        }

        public Data.Pack db { get {
            if (_db == null)
                _db = new Data.Pack();
            return _db;
        } }
        Data.Pack _db;
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "OnGet";
        }
        //public void OnPost()
        //{
        //    Message = "OnPost";
        //}

        public void OnPostHandler1()
        {
            Message = "OnPostHandler";
        }
    }
}

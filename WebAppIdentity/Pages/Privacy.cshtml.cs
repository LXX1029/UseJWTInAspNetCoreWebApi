using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public PrivacyModel(ILogger<PrivacyModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            this._environment = environment;
        }

        [BindProperty]
        public string Message { get; set; }

        /// <summary>
        /// 接收的路由参数
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string RouteValue { get; set; }
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


        //[BindProperty(SupportsGet = true)]
        //public string[] CategoryId { get; set; } = new string[0];

        public void OnPostHandleBindCollection(string[] categoryId)
        {
            // 模拟设置Cookie
            var cookieOption = new CookieOptions
            {
                Expires = DateTime.Now.AddSeconds(5)
            };
            Response.Cookies.Append("mycookie", "value1", cookieOption);

            // 设置Session
            HttpContext.Session.SetString("session1", "session1");
        }

        [PageRemote(
             ErrorMessage = "值已存在",
            PageHandler = "CheckRemoteValidation",
            AdditionalFields = "__RequestVerificationToken",
            HttpMethod = "post"
            )]
        [BindProperty]
        public string RemoteValidation { get; set; }

        public JsonResult OnPostCheckRemoteValidation()
        {
            var existingNumbers = new[] { "1", "2", "6" };

            return new JsonResult(!existingNumbers.Contains(RemoteValidation)); ;
        }

      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppIdentity.Data.Services;
using WebAppIdentity.Models;

namespace WebAppIdentity.Pages.Recipes
{
    public class AddRecipeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRecipeService _recipeService;

        public AddRecipeModel(IRecipeService recipeService, UserManager<ApplicationUser> userManager)
        {
            this._recipeService = recipeService;
            this._userManager = userManager;
        }
        public List<SelectListItem> Options { get; set; } = new List<SelectListItem>();
        public void OnGet()
        {
            Options.Add(new SelectListItem { Value = "1", Text = "1" });
            Options.Add(new SelectListItem { Value = "2", Text = "2" });
            Options.Add(new SelectListItem { Value = "3", Text = "3" });
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        /// <summary>
        /// asp-for 属性绑定到一个double[] 数组是，前端将渲染一个列表框，且支持多选值。
        /// </summary>
        [BindProperty]
        public double[] SelectedPrice { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                return Page();
            }

            var appUser = await this._userManager.GetUserAsync(this.User);
            this.Recipe.CreatedById = appUser.Id;
            await this._recipeService.CreateRecipe(Recipe);

            #region 测试邮件发送
            //var body = $@"测试邮件发送内容，需在生成客户端授权码。";
            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential("dwjams@qq.com", "kartnwkggstmbdje");
            //    smtp.Credentials = credential;
            //    smtp.Host = "smtp.qq.com";
            //    smtp.EnableSsl = true;
            //    smtp.UseDefaultCredentials = false;
            //    var message = new MailMessage();
            //    message.To.Add("duanzipeng@jiean.net");
            //    message.Subject = "测试邮件发送";
            //    message.Body = body;
            //    //message.IsBodyHtml = true;
            //    message.From = new MailAddress("dwjams@qq.com");
            //    await smtp.SendMailAsync(message);
            //}
            #endregion


            return RedirectToPage("/Index");
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
        }
        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {

            await next.Invoke();
        }
    }
}
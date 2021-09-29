using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppIdentity.Models
{
    /// <summary>
    /// 授权条件
    /// </summary>
    public class NameRequirement : IAuthorizationRequirement
    {
        public NameRequirement(string nameKey)
        {
            this.NameKey = nameKey;
        }
        /// <summary>
        /// 登录名中包含的关键字
        /// </summary>
        public string NameKey { get; set; }
    }
    /// <summary>
    /// Name条件 授权处理
    /// </summary>
    public class NameRequerementHandler : AuthorizationHandler<NameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NameRequirement requirement)
        {
            if (context.User.Identity.Name.Contains(requirement.NameKey))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class IsRecipeOwnerRequirement : IAuthorizationRequirement { }

    public class IsRecipeOwnerHandler : AuthorizationHandler<IsRecipeOwnerRequirement, Recipe>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IsRecipeOwnerHandler(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsRecipeOwnerRequirement requirement, Recipe resource)
        {
            var appUser = await this._userManager.GetUserAsync(context.User);
            if (appUser == null) return;
            if (resource.CreatedById == appUser.Id)
                context.Succeed(requirement);
        }
    }
}

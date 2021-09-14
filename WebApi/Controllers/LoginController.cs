using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public LoginController(ITokenService tokenService)
        {
            this._tokenService = tokenService;
        }
        [HttpPost]
        public IActionResult Login([FromForm] string user, [FromForm] string password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }
            // 验证user/password
            if (user == "admin" && password == "admin")
            {
                var token = this._tokenService.GenerateToken(user);
                return Ok(token);
            }
            return BadRequest("用户名或密码错误");
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<int>> GetList()
        {
            var claims = HttpContext.User.Claims;
            return Ok(Enumerable.Range(1, 10).ToList());
        }
    }
}

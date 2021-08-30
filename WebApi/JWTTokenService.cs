using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public interface ITokenService
    {
        bool Authenticate(string user, string password, out string token);
    }

    public class JWTTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private const int KEY_SIZE = 32;
        private Microsoft.IdentityModel.Tokens.SymmetricSecurityKey _securityKey;
        private const double VALID_FOR_MINUTES = 0.5;
        public JWTTokenService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.InitializeCrypto();
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns>SymmetricSecurityKey</returns>
        public SymmetricSecurityKey GetSecurityKey() => _securityKey;

        /// <summary>
        ///  重新生成密钥
        /// </summary>
        private void InitializeCrypto()
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[KEY_SIZE];
            cryptoProvider.GetBytes(randomBytes, 0, KEY_SIZE);
            _securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(randomBytes);
        }

        /// <summary>
        /// 认证用户
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="token">token</param>
        /// <returns>bool</returns>
        public bool Authenticate(string user, string password, out string token)
        {
            bool result = false;
            token = null;
            // 或者 查询数据库
            if (user == "admin" && password == "admin")
            {
                var credentials = new SigningCredentials(this._securityKey, SecurityAlgorithms.HmacSha256);
                var expires = this._configuration.GetValue<double>("JwtExpire");
                DateTime expiresOn = DateTime.Now.AddMinutes(expires);
                var claims = new Claim[] {
                  new Claim(ClaimTypes.NameIdentifier,user),
                  new Claim(ClaimTypes.Expiration,expiresOn.ToLongDateString())
                };
                var issuer = Environment.MachineName;
                var jwtToken = new JwtSecurityToken(issuer: issuer, audience: issuer, claims, null, expiresOn, credentials);
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                result = true;
            }
            return result;
        }
    }
}

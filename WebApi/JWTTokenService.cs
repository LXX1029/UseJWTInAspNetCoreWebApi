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
        string GenerateToken(string userName);
    }

    public class JWTTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private const int KEY_SIZE = 32;
        private SymmetricSecurityKey _securityKey;
        public JWTTokenService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.InitializeSecurityKey();
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns>SymmetricSecurityKey</returns>
        public SymmetricSecurityKey GetSecurityKey() => _securityKey;

        /// <summary>
        ///  重新生成密钥
        /// </summary>
        private void InitializeSecurityKey()
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[KEY_SIZE];
            cryptoProvider.GetBytes(randomBytes, 0, KEY_SIZE);
            _securityKey = new SymmetricSecurityKey(randomBytes);
        }

        /// <summary>
        /// 认证用户,生成Token
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>string</returns>
        public string GenerateToken(string userName)
        {
            JwtSecurityToken jwtToken;
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentNullException("userName 为空");
                var credentials = new SigningCredentials(this._securityKey, SecurityAlgorithms.HmacSha256);
                var expires = this._configuration.GetValue<double>("JwtExpire");
                DateTime expiresOn = DateTime.Now.AddMinutes(expires);
                var claims = new Claim[] {
                  new Claim(ClaimTypes.NameIdentifier,userName),
                  new Claim(ClaimTypes.Expiration,expiresOn.ToLongDateString())
                };
                var issuer = Environment.MachineName;
                jwtToken = new JwtSecurityToken(issuer: issuer, audience: issuer, claims, null, expiresOn, credentials);
            }
            catch (Exception)
            {
                throw new Exception("生成 Token 值失败");
            }
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}

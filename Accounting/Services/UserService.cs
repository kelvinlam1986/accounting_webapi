using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Accounting.Infrastructure.Core;
using Accounting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Accounting.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private string _key = "private_key_1234567890";
        private readonly AppSettings _appSettings;

        public UserService(UserManager<User> userManager, IOptions<AppSettings> appSettings)
        {
            this._userManager = userManager;
            this._appSettings = appSettings.Value;
        }

        public UserToken Authenticate(string username, string password)
        {
            // check if there's an user with the given username
            var user = this._userManager.FindByNameAsync(username).Result;
            // fallback to support e-mail address instead of username
            if (user == null && username.Contains("@"))
            {
                user = this._userManager.FindByEmailAsync(username).Result;
            }

            if (user == null)
            {
                return null;
            }

            var success = user != null &&
                   this._userManager.CheckPasswordAsync(user, password).Result;

            if (!success)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._appSettings.Secret);
            var timeExpire = TimeSpan.FromDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(timeExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new UserToken { Username = username, Token = tokenHandler.WriteToken(token), TokenExpire = (int)timeExpire.TotalSeconds };
        }
    }
}

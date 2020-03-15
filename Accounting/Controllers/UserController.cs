using Accounting.Models;
using Accounting.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    [Produces("application/json")]
    [Route("api/connect")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Authenticate([FromBody]UserToken userParam)
        {
            var token = _userService.Authenticate(userParam.Username, userParam.Password);
            if (token == null)
                return BadRequest(new { message = "Tên đăng nhập hoặc mật khẩu không đúng." });

            var jwt = new
            {
                access_token = token.Token,
                expiration = (int)token.TokenExpire,
            };

            return Ok(jwt);
        }

    }
}
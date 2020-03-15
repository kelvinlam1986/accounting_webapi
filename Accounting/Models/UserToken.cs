namespace Accounting.Models
{
    public class UserToken
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int TokenExpire { get; set; }
    }
}

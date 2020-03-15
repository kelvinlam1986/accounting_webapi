using Accounting.Models;

namespace Accounting.Services
{
    public interface IUserService
    {
        UserToken Authenticate(string username, string password);
    }
}

using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}

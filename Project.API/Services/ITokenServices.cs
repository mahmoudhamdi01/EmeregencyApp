using Project.Core.Entities;

namespace Project.API.Services
{
    public interface ITokenServices
    {
        Task<string> CreateToken(User user);
    }
}

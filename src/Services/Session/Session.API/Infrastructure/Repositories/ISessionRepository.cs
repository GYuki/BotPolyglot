using System.Threading.Tasks;
using Session.API.Model;

namespace Session.API.Infrastructure.Repositories
{
    public interface ISessionRepository
    {
        Task<SessionModel> GetSessionAsync(int chatId, AuthType auth);
        Task<SessionModel> UpdateSessionAsync(SessionModel session);
        Task<bool> DeleteSessionAsync(int chatId, AuthType auth);
    }
}
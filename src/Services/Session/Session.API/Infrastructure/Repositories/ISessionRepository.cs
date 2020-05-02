using System.Threading.Tasks;
using Session.API.Model;

namespace Session.API.Infrastructure.Repositories
{
    public interface ISessionRepository
    {
        Task<SessionModel> GetSessionAsync(long chatId, AuthType auth);
        Task<SessionModel> UpdateSessionAsync(SessionModel session);
        Task<bool> DeleteSessionAsync(long chatId, AuthType auth);
    }
}
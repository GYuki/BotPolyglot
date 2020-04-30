using System.Threading.Tasks;
using ApiGateways.Telegram.Sender.Models;

namespace ApiGateways.Telegram.Sender.Infrastructure.Services
{
    public interface ISessionService
    {
        Task<SessionData> GetSessionDataAsync(AuthType authType, int chatId);
        Task UpdateOrCreateSessionAsync(SessionData sessionData);
    }
}
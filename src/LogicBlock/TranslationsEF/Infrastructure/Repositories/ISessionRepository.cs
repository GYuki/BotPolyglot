using System.Threading.Tasks;

namespace LogicBlock.Session.Repositories
{
    public interface ISessionRepository
    {
        Task<Session> GetSessionAsync(int chatId);
    }
}
using System.Threading.Tasks;

namespace LogicBlock.Session.Repositories
{
    public interface ISessionRepository
    {
        Task<ChatSession> GetSessionAsync(int chatId);
    }
}
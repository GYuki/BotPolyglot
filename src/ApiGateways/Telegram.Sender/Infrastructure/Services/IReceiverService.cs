using System.Threading.Tasks;
using ApiGateways.Telegram.Sender.Models;

namespace ApiGateways.Telegram.Sender.Infrastructure.Services
{
    public interface IReceiverService
    {
        Task<ReceiverResponse> HandleReceiverRequestAsync(ActionRequest request);
        Task<ReceiverResponse> HandleAfterActionRequestAsync(SessionData session); 
    }
}
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateways.Telegram.Sender.Config;
using ApiGateways.Telegram.Sender.Models;
using Microsoft.Extensions.Options;
using GrpcReceiver;
using static GrpcReceiver.Receiver;
using System.Linq;

namespace ApiGateways.Telegram.Sender.Infrastructure.Services
{
    public class ReceiverService : IReceiverService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;

        public ReceiverService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task<ReceiverResponse> HandleAfterActionRequestAsync(SessionData session)
        {
            return await GrpcCallerService.CallService(_urls.Receiver, async channel =>
            {
                var client = new ReceiverClient(channel);
                var request = MapToChatRequest(session);
                var response = await client.HandleAfterActionAsync(request);
                return MapToReceiverResponse(response);
            });
        }

        public async Task<ReceiverResponse> HandleReceiverRequestAsync(Models.ActionRequest actionRequest)
        {
            return await GrpcCallerService.CallService(_urls.Receiver, async channel =>
            {
               var client = new ReceiverClient(channel);
               var request = MapToGrpcActionRequest(actionRequest);
               var response = await client.HandleReceiverRequestAsync(request);
               return MapToReceiverResponse(response);
            });
        }

        private ReceiverResponse MapToReceiverResponse(ChatResponse response)
        {
            return new ReceiverResponse
            {
                Message = response.Message,
                Success = response.Success,
                SessionData = MapToSessionData(response.Session)
            };
        }

        private SessionData MapToSessionData(ChatRequest request)
        {
            return new SessionData
            {
                ExpectedWord = request.ExpectedWord,
                Language = request.Language,
                State = (State)request.State,
                WordSequence = request.WordSequence != null ? request.WordSequence.ToList() : default
            };
        }

        private GrpcReceiver.ActionRequest MapToGrpcActionRequest(Models.ActionRequest request)
        {
            return new GrpcReceiver.ActionRequest
            {
                Message = request.Message,
                Request = MapToChatRequest(request.SessionData)
            };
        }

        private GrpcReceiver.ChatRequest MapToChatRequest(SessionData session)
        {
            var result = new GrpcReceiver.ChatRequest
            {
                ExpectedWord = session.ExpectedWord,
                Language = session.Language,
                State = (int)session.State
            };

            if (session.WordSequence != null)
                session.WordSequence.ForEach(x => result.WordSequence.Add(x));
            
            return result;
        }
    }
}
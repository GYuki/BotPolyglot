using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateways.Telegram.Sender.Config;
using ApiGateways.Telegram.Sender.Models;
using GrpcSession;
using Microsoft.Extensions.Options;
using static GrpcSession.Session;

namespace ApiGateways.Telegram.Sender.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly HttpClient _httpClient;
        private readonly UrlsConfig _urls;
        
        public SessionService(HttpClient httpClient, IOptions<UrlsConfig> config)
        {
            _httpClient = httpClient;
            _urls = config.Value;
        }

        public async Task<SessionData> GetSessionDataAsync(Models.AuthType authType, int chatId)
        {
            return await GrpcCallerService.CallService(_urls.Session, async channel =>
            {
                var client = new SessionClient(channel);
                var request = new GetSessionRequest
                {
                    AuthType = GrpcSession.AuthType.Telegram,
                    ChatId = chatId
                };
                var response = await client.GetSessionByIdAsync(request);
                return MapToSessionData(response);
            });
        }

        public async Task UpdateOrCreateSessionAsync(SessionData sessionData)
        {
            await GrpcCallerService.CallService(_urls.Session, async channel =>
            {
                var client = new SessionClient(channel);
                var request = MapToSessionRequest(sessionData);
                var response = await client.UpdateSessionAsync(request);
            });
        }

        private SessionRequest MapToSessionRequest(SessionData sessionData)
        {
            var result = new SessionRequest
            {
                AuthType = (GrpcSession.AuthType)sessionData.AuthType,
                ChatId = sessionData.ChatId,
                ExpectedWord = sessionData.ExpectedWord,
                State = (GrpcSession.State)sessionData.State
            };

            if (sessionData.WordSequence != null)
                foreach(var w in sessionData.WordSequence)
                    result.WordSequence.Add(w);

            return result;
        }

        private SessionData MapToSessionData(SessionRequest request)
        {
            return new SessionData
            {
                AuthType = (Models.AuthType)request.AuthType,
                ChatId = request.ChatId,
                ExpectedWord = request.ExpectedWord,
                State = (Models.State)request.State,
                WordSequence = request.WordSequence != null ? request.WordSequence.ToList() : default
            };
        }
    }
}
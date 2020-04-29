using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Telegram.Bot.Args;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Sender.Telegram.API.Infrastructure.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://api.telegram.org/bot";

        public TelegramService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }

        private async Task<TResponse> MakeRequestAsync<TResponse>(
            string botToken,
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            string url = $"{BASE_URL}{botToken}/{request.MethodName}";

            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = request.ToHttpContent()
            };

            HttpResponseMessage httpResponse = default;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw;
            }

            var actualResponseStatusCode = httpResponse.StatusCode;
            string responseJson = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            
            switch(actualResponseStatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Forbidden when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Conflict when !string.IsNullOrWhiteSpace(responseJson):
                    break;
                default:
                    httpResponse.EnsureSuccessStatusCode();
                    break;
            }

            var apiResponse = 
                JsonConvert.DeserializeObject<ApiResponse<TResponse>>(responseJson)
                ?? new ApiResponse<TResponse>
                {
                    Ok = false,
                    Description = "No response received"
                };
            
            return apiResponse.Result;
        }
        public Task<Message> SendTextMessageAsync(
            string botToken,
            ChatId chatId,
            string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                botToken,
                new SendMessageRequest(chatId, text)
            {
                ParseMode = parseMode,
                DisableNotification = disableNotification,
                DisableWebPagePreview = disableWebPagePreview,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);
    }
}
using System.Net;
using System.Threading.Tasks;
using ApiGateways.Telegram.Sender.Extensions;
using ApiGateways.Telegram.Sender.Infrastructure.Services;
using ApiGateways.Telegram.Sender.Models;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ApiGateways.Telegram.Sender.Controllers
{
    [Produces("applictation/json")]
    [Route("gw/[controller]")]
    [ApiController]
    public class TelegramController : Controller
    {
        private readonly ITelegramService _telegram;
        private readonly ISessionService _session;
        private readonly IReceiverService _receiver;

        public TelegramController(
            ITelegramService telegram,
            ISessionService session,
            IReceiverService receiver
            )
        {
            _telegram = telegram;
            _session = session;
            _receiver = receiver;
        }

        [HttpPost]
        [Route("{token}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> HandleMessageAsync(string token, [FromBody]Update update)
        {
            var message = "";
            var currentSession = await _session.GetSessionDataAsync(
                AuthType.Telegram,
                update.Message.Chat.Id
            ) ?? new SessionData
            {
                AuthType = AuthType.Telegram,
                ChatId = update.Message.Chat.Id,
                State = State.Idle
            };

            var messageText = "";

            if (update.Message.Entities != null)
            {
                messageText = update.Message.Text.Substring(
                    update.Message.Entities[0].Offset,
                    update.Message.Entities[0].Length
                );
            }
            else
                messageText = update.Message.Text;

            var receiverResponse = await _receiver.HandleReceiverRequestAsync(new ActionRequest
            {
                Message = messageText,
                SessionData = currentSession
            });

            message += receiverResponse.Message;

            currentSession.Update(receiverResponse.SessionData);

            var afterAction = await _receiver.HandleAfterActionRequestAsync(currentSession);

            if (afterAction != null)
                message += "\n" + afterAction.Message;

            await _session.UpdateOrCreateSessionAsync(currentSession);

            await _telegram.SendTextMessageAsync(token, update.Message.Chat.Id, message);

            return Ok();
        }
    }
}
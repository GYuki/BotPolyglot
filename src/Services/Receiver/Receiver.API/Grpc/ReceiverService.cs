using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using LogicBlock.Info;
using LogicBlock.Logic;
using LogicBlock.Session;
using Microsoft.Extensions.Logging;

namespace GrpcReceiver
{
    public class ReceiverService : Receiver.ReceiverBase
    {
        private readonly IArcadeLogic _arcade;
        private readonly ITutorialLogic _tutorial;
        private readonly ILogger<ReceiverService> _logger;

        public ReceiverService(IArcadeLogic arcade, ITutorialLogic tutorial)
        {
            _arcade = arcade;
            _tutorial = tutorial;
        }

        public override async Task<ChatResponse> HandleArcadeStart(ChatRequest request, ServerCallContext context)
        {
            _logger.LogInformation("ReceiverService.HandleArcadeStart()");

            ChatSession chatSession = MapToChatSession(request);

            StartRequestInfo info = new StartRequestInfo
            {
                Request = new StartRequest(chatSession)
            };

            var logicResult = await _arcade.StartChat(info);

            var response = MapToChatResponse(logicResult, chatSession);

            return response;
        }

        public override async Task<ChatResponse> HandleTutorialStart(ChatRequest request, ServerCallContext context)
        {
            _logger.LogInformation("ReceiverService.HandleTutorialStart()");

            ChatSession chatSession = MapToChatSession(request);

            StartRequestInfo info = new StartRequestInfo
            {
                Request = new StartRequest(chatSession)
            };

            var logicResult = await _tutorial.StartChat(info);

            var response = MapToChatResponse(logicResult, chatSession);

            return response;
        }

        public override async Task<ChatResponse> HandleArcadeAction(ActionRequest action, ServerCallContext context)
        {
            _logger.LogInformation("ReceiverService.HandleArcadeAction()");

            ChatSession chatSession = MapToChatSession(action.Request);

            TextRequestInfo info = new TextRequestInfo
            {
                Request = new TextRequest(chatSession, action.Message)
            };

            var logicResult = await _arcade.HandleText(info);

            var response = MapToChatResponse(logicResult, chatSession);

            return response;
        }

        public override async Task<ChatResponse> HandleTutorialAction(ActionRequest action, ServerCallContext context)
        {
            _logger.LogInformation("ReceiverService.HandleTutorialAction()");

            ChatSession chatSession = MapToChatSession(action.Request);

            TextRequestInfo info = new TextRequestInfo
            {
                Request = new TextRequest(chatSession, action.Message)
            };

            var logicResult = await _tutorial.HandleText(info);

            var response = MapToChatResponse(logicResult, chatSession);

            return response;
        }

        private ChatSession MapToChatSession(ChatRequest request)
        {
            return new ChatSession
            {
                ExpectedWord = request.ExpectedWord,
                WordSequence = request.WordSequence.ToList()
            };
        }

        private ChatRequest MapToChatRequest(ChatSession session)
        {
            var result = new ChatRequest
            {
                ExpectedWord = session.ExpectedWord
            };

            session.WordSequence.ForEach(word => result.WordSequence.Add(word));

            return result;
        }

        private ChatResponse MapToChatResponse(IResponseInfo responseInfo, ChatSession session)
        {
            return new ChatResponse
            {
                Message = responseInfo.Message,
                Session = MapToChatRequest(session)
            };
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using LogicBlock.Info;
using LogicBlock.Logic;
using LogicBlock.Session;
using Microsoft.Extensions.Logging;
using Receiver.API.Infrastructure.LogicController;
using Receiver.API.Models;

namespace GrpcReceiver
{
    public class ReceiverService : Receiver.ReceiverBase
    {
        private readonly IArcadeLogic _arcade;
        private readonly ITutorialLogic _tutorial;

        private readonly ILogicController _logic;
        private readonly ILogger<ReceiverService> _logger;

        public ReceiverService(IArcadeLogic arcade, ITutorialLogic tutorial)
        {
            _arcade = arcade;
            _tutorial = tutorial;
        }
        
        public ReceiverService(ILogicController logic, ILogger<ReceiverService> logger)
        {
            _logic = logic;
            _logger = logger;
        }

        public override async Task<ChatResponse> HandleReceiverRequest(ActionRequest action, ServerCallContext context)
        {
            _logger.LogInformation("receiverService.HandleReceiverRequest()");

            ChatSession chatSession = MapToChatSession(action.Request);

            var currentState = _logic.GetLogic(chatSession.State);

            if (currentState == null)
            {
                context.Status = new Status(StatusCode.Internal, $"State {chatSession.State.ToString()} is not found in receiver logic controller");
                return null;
            }

            var response = new ChatResponse()
            {
                Session = MapToChatRequest(chatSession),
                Success = true
            };

            switch(action.Message)
            {
                case "back":
                    response.Message = currentState.Back(chatSession);
                    break;
                case "menu":
                    response.Message = currentState.Menu(chatSession);
                    break;
                default:
                    var actionResult = await currentState.Act(action.Message, chatSession);
                    response = ResponseModelToChatResponse(actionResult);
                    break;
            }

            context.Status = new Status(StatusCode.OK, $"Success action");
            return response;
        }

        public override async Task<ChatResponse> HandleAfterAction(ChatRequest request, ServerCallContext context)
        {
            _logger.LogInformation("ReceiverService.HandleAfterAction()");

            ChatSession chatSession = MapToChatSession(request);

            var currentState = _logic.GetActionLogic(chatSession.State);

            if (currentState == null)
            {
                context.Status = new Status(StatusCode.Internal, $"State {chatSession.State.ToString()} is not found in receiver logic controller");
                return null;
            }

            var result = await currentState.GetNextTask(chatSession);
            context.Status = new Status(StatusCode.OK, $"Success AfterAction");
            return new ChatResponse
            {
                Message = result,
                Session = MapToChatRequest(chatSession),
                Success = true
            };

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

        
        private ChatResponse ResponseModelToChatResponse(ResponseModel model)
        {
            return new ChatResponse
            {
                Message = model.Message,
                Session = MapToChatRequest(model.Session),
                Success = model.Success
            };
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
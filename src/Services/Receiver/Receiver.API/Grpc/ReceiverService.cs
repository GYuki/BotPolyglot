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
        private readonly ILogicController _logic;
        private readonly ILogger<ReceiverService> _logger;
        
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
                    response.Message = await currentState.Back(chatSession);
                    break;
                case "menu":
                    response.Message = await currentState.Menu(chatSession);
                    break;
                default:
                    var actionResult = await currentState.Act(action.Message, chatSession);
                    response = ResponseModelToChatResponse(actionResult);
                    break;
            }

            response.Session = MapToChatRequest(chatSession);
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
        private ChatSession MapToChatSession(ChatRequest request)
        {
            return new ChatSession
            {
                ExpectedWord = request.ExpectedWord,
                WordSequence = request.WordSequence.ToList(),
                State = (State)request.State,
                Language = request.Language
            };
        }

        private ChatRequest MapToChatRequest(ChatSession session)
        {
            var result = new ChatRequest
            {
                ExpectedWord = session.ExpectedWord,
                Language = session.Language,
                State = (int)session.State
            };

            if (session.WordSequence != null)
                session.WordSequence.ForEach(word => result.WordSequence.Add(word));

            return result;
        }

        
        private ChatResponse ResponseModelToChatResponse(ResponseModel model)
        {
            return new ChatResponse
            {
                Message = model.Message,
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
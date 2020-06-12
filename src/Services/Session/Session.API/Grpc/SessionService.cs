using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Session.API.Infrastructure.Repositories;
using Session.API.Model;
using models = Session.API.Model;

namespace GrpcSession
{
    public class SessionService : Session.SessionBase
    {
        private readonly ISessionRepository _repository;

        public SessionService(ISessionRepository repository)
        {
            _repository = repository;
        }

        public override async Task<SessionRequest> GetSessionById(GetSessionRequest request, Grpc.Core.ServerCallContext context)
        {
            var data = await _repository.GetSessionAsync(request.ChatId, (models.AuthType)request.AuthType);

            if (data != null)
            {
                context.Status = new Status(StatusCode.OK, $"Session {request.AuthType.ToString()}_{request.ChatId.ToString()} exists");

                return MapToSessionRequest(data);
            }

            return null;
        }

        public override async Task<Empty> UpdateSession(SessionRequest request, Grpc.Core.ServerCallContext context)
        {
            var sessionModel = MapToSessionModel(request);

            var response = await _repository.UpdateSessionAsync(sessionModel);

            if (response != null)
                return new Empty();
            
            context.Status = new Status(StatusCode.NotFound, $"Session {request.AuthType.ToString()}_{request.ChatId.ToString()} does not exist");

            return new Empty();
        }

        public override async Task<Empty> DeleteSession(SessionRequest request, ServerCallContext context)
        {
            var sessionModel = MapToSessionModel(request);

            var response = await _repository.DeleteSessionAsync(sessionModel.ChatId, sessionModel.AuthType);

            if (response)
                return new Empty();
            
            context.Status = new Status(StatusCode.NotFound, $"Session {request.AuthType.ToString()}_{request.ChatId.ToString()} does not exist");

            return new Empty();
        }

        private SessionModel MapToSessionModel(SessionRequest request)
        {
            return new SessionModel
            {
                AuthType = (models.AuthType)request.AuthType,
                ChatId = request.ChatId,
                ExpectedWord = request.ExpectedWord,
                State = (models.State)request.State,
                WordSequence = request.WordSequence.ToList(),
                Award = request.Award,
                Language = request.Language
            };
        }

        private SessionRequest MapToSessionRequest(models.SessionModel session)
        {
            var result = new SessionRequest
            {
                AuthType = (AuthType)session.AuthType,
                ChatId = session.ChatId,
                ExpectedWord = session.ExpectedWord,
                State = (State)session.State,
                Award = session.Award,
                Language = session.Language
            };

            session.WordSequence.ForEach(x => result.WordSequence.Add(x));

            return result;
        }
    }
}
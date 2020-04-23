using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Logic;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class ArcadeActionLogic : BaseLogic, IArcadeActionLogic
    {
        private readonly IArcadeLogic _arcade;
        public ArcadeActionLogic(IArcadeLogic arcade)
        {
            _arcade = arcade;
        }
        public override async Task<ResponseModel> Act(string message, LogicBlock.Session.ChatSession session)
        {
            var result = new ResponseModel();

            IResponseInfo responseInfo;

            if (session.WordSequence == null)
            {
                StartRequestInfo info = new StartRequestInfo
                {
                    Request = new StartRequest(session)
                };

                responseInfo = await _arcade.StartChat(info);
            }
            else
            {
                TextRequestInfo info = new TextRequestInfo
                {
                    Request = new TextRequest(session, message)
                };

                responseInfo = await _arcade.HandleText(info);
            }

            result.Message = responseInfo.Message;
            result.Session = session;
            result.Success = responseInfo.Success;
            return result;
        }
    }
}
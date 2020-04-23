using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Logic;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class TutorialActionLogic : BaseLogic, ITutorialActionLogic
    {
        private readonly ITutorialLogic _tutorial;

        public TutorialActionLogic(ITutorialLogic tutorial)
        {
            _tutorial = tutorial;
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

                responseInfo = await _tutorial.StartChat(info);
            }
            else
            {
                TextRequestInfo info = new TextRequestInfo
                {
                    Request = new TextRequest(session, message)
                };

                responseInfo = await _tutorial.HandleText(info);
            }

            result.Message = responseInfo.Message;
            result.Session = session;
            result.Success = responseInfo.Success;

            return result;
        }
    }
}
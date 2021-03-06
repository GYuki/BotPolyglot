using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Logic;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Utils;
using Receiver.API.Models;

namespace Receiver.API.States
{
    public class ArcadeActionLogic : BaseActionLogic, IArcadeActionLogic
    {
        public ArcadeActionLogic(IArcadeLogic arcade, ITranslationsRepository translation)
            :base(arcade, translation)
        {       
        }
        public override async Task<ResponseModel> Act(string message, ChatSession session)
        {
            var result = new ResponseModel();

            IResponseInfo responseInfo;

            if (session.WordSequence == null)
            {
                StartRequestInfo info = new StartRequestInfo
                {
                    Request = new StartRequest(session)
                };

                responseInfo = await _logic.StartChat(info);
            }
            else
            {
                TextRequestInfo info = new TextRequestInfo
                {
                    Request = new TextRequest(session, message)
                };

                responseInfo = await _logic.HandleText(info);
            }

            result.Message = responseInfo.Message;
            result.Session = session;
            result.Success = responseInfo.ResponseCode == ResponseCodes.OK;
            return result;
        }
    }
}
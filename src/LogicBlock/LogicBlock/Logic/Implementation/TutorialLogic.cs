using System.Threading.Tasks;
using LogicBlock.Info;

namespace LogicBlock.Logic
{
    public class TutorialLogic : ITutorialLogic
    {
        public async Task<IResponseInfo> StartChat(IStartRequestInfo info)
        {
            return null;
        }

        public async Task<IResponseInfo> HandleText(ITextRequestInfo info)
        {
            return null;
        }
    }
}
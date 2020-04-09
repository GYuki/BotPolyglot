using LogicBlock.Session;

namespace LogicBlock.Info
{
    public class StartRequest : IStartOperationRequest
    {
        public ChatSession Session => _session;

        private readonly ChatSession _session;
        
        public StartRequest(ChatSession session)
        {
            _session = session;
        }
    }
}
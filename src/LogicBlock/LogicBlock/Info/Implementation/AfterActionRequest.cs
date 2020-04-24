using LogicBlock.Session;

namespace LogicBlock.Info
{
    public class AfterActionRequest : IAfterActionOperationRequest
    {
        public ChatSession Session => _session;

        private readonly ChatSession _session;

        public AfterActionRequest(ChatSession session)
        {
            _session = session;
        }
    }
}
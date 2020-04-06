using LogicBlock.Session;

namespace LogicBlock.Info
{
    public class TextRequest : ITextOperationRequest
    {
        public ChatSession Session => _session;
        public string MessageText => _messageText;
        private readonly ChatSession _session;
        private readonly string _messageText;

        public TextRequest(ChatSession session, string messageText)
        {
            _session = session;
            _messageText = messageText;
        }
    }
}
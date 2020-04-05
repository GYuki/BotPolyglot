namespace LogicBlock.Info
{
    public class TextRequest : ITextOperationRequest
    {
        public Session.Session Session => _session;
        public string MessageText => _messageText;
        private readonly Session.Session _session;
        private readonly string _messageText;

        public TextRequest(Session.Session session, string messageText)
        {
            _session = session;
            _messageText = messageText;
        }
    }
}
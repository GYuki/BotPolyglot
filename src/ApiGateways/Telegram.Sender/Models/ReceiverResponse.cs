namespace ApiGateways.Telegram.Sender.Models
{
    public class ReceiverResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public SessionData SessionData { get; set; }
    }
}
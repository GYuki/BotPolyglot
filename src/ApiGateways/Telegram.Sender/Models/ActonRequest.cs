namespace ApiGateways.Telegram.Sender.Models
{
    public class ActionRequest
    {
        public string Message { get; set; }
        public SessionData SessionData { get; set; }
    }
}
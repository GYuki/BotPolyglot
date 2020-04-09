using LogicBlock.Session;

namespace Receiver.API.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public bool Success { get; set;  }
        public ChatSession Session { get; set; }
    }
}
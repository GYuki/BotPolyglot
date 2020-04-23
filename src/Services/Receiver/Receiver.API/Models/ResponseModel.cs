using LogicBlock.Session;

namespace Receiver.API.Models
{
    public class ResponseModel
    {
        public string Message { get; set; }
        public bool Success
        {
            get
            {
                return _success;
            }
            set
            {
                _success = value;
            }
        }
        public ChatSession Session { get; set; }

        private bool _success = true;
    }
}
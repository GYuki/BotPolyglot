using LogicBlock.Info;

namespace LogicBlock.Logic
{
    public class ArcadeResponseInfo : IArcadeResponseInfo
    {
        public string Message { get; private set; }
        public bool Success { get; private set; }
        public int Award { get; private set; }

        public ArcadeResponseInfo(string message, bool success, int award = 0)
        {
            Message = message;
            Award = 0;
            Success = success;
        }
    }

    public class StartResponseInfo : IStartResponseInfo
    {
        public bool Success => _success;
        public string Message => _message;

        private readonly bool _success;
        private readonly string _message;

        public StartResponseInfo(bool success, string message)
        {
            _success = success;
            _message = message;
        }
    }

    public class AfterActionResponseInfo : IAfterActionResponseInfo
    {
        public bool Success => _success;
        public string Message => _message;

        private readonly bool _success;
        private readonly string _message;

        public AfterActionResponseInfo(bool success, string message)
        {
            _success = success;
            _message = message;
        }
    }
}
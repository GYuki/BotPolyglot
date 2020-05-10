using LogicBlock.Info;

namespace LogicBlock.Logic
{
    public class ArcadeResponseInfo : IArcadeResponseInfo
    {
        public string Message { get; private set; }
        public byte ResponseCode { get; private set; }
        public int Award { get; private set; }

        public ArcadeResponseInfo(string message, byte responseCode, int award = 0)
        {
            Message = message;
            Award = 0;
            ResponseCode = responseCode;
        }
    }

    public class StartResponseInfo : IStartResponseInfo
    {
        public byte ResponseCode => _responseCode;
        public string Message => _message;

        private readonly byte _responseCode;
        private readonly string _message;

        public StartResponseInfo(byte responseCode, string message)
        {
            _responseCode = responseCode;
            _message = message;
        }
    }

    public class AfterActionResponseInfo : IAfterActionResponseInfo
    {
        public byte ResponseCode => _responseCode;
        public string Message => _message;

        private readonly byte _responseCode;
        private readonly string _message;

        public AfterActionResponseInfo(byte responseCode, string message)
        {
            _responseCode = responseCode;
            _message = message;
        }
    }
}
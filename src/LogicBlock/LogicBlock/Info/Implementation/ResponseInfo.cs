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

        private readonly bool _success;

        public StartResponseInfo(bool success)
        {
            _success = success;
        }
    }
}
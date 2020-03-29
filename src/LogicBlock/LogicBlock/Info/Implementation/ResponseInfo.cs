using LogicBlock.Info;

namespace LogicBlock.Logic
{
    public class ArcadeResponseInfo : IArcadeResponseInfo
    {
        public string Message { get; private set; }
        public Session.Session Session { get; }
        public int Award { get; private set; }

        public ArcadeResponseInfo(string message, int award = 0)
        {
            Message = message;
            Award = 0;
        }
    }
}
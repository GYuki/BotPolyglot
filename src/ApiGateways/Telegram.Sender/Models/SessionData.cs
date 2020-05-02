using System.Collections.Generic;

namespace ApiGateways.Telegram.Sender.Models
{
    public class SessionData
    {
        public int ExpectedWord { get; set; }
        public List<int> WordSequence { get; set; }
        public AuthType AuthType { get; set; }
        public long ChatId { get; set; }
        public State State { get; set; }
        public string Language { get; set; }
    }

    public enum AuthType 
    {
        Telegram,
        VK
    }

    public enum State
    {
        Idle,
        LanguageChoose,
        ModeChoose,
        ArcadeAction,
        TutorialAction
    }
}
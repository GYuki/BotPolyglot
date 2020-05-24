using System.Collections.Generic;

namespace LogicBlock.Session
{
    public class ChatSession
    {
        public int ExpectedWord { get; set; }
        public List<int> WordSequence { get; set; }
        public State State { get; set; }
        public string Language { get; set; }
        public int Award { get; set; }
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
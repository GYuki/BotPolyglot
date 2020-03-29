using System.Collections.Generic;

namespace LogicBlock.Session
{
    public class Session
    {
        public int ChatId { get; set; }
        public int LastMessage { get; set; }
        public int PlayMode { get; set; }
        public int ExpectedWord { get; set; }
        public List<int> WordSequence { get; set; }
    }
}
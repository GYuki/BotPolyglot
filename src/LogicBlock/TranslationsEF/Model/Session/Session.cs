using System.Collections.Generic;

namespace LogicBlock.Session
{
    public class ChatSession
    {
        public int ExpectedWord { get; set; }
        public List<int> WordSequence { get; set; }
    }
}
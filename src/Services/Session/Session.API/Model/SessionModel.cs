using System.Collections.Generic;

namespace Session.API.Model
{
    public class SessionModel
    {
        public int ExpectedWord { get; set; }

        public List<int> WordSequence { get; set; }

        public AuthType AuthType { get; set; }

        public int ChatId { get; set; }

        public State State { get; set; }
        public string Language { get; set; }
    }
}
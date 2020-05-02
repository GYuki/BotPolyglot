using System;
using ApiGateways.Telegram.Sender.Models;

namespace ApiGateways.Telegram.Sender.Extensions
{
    public static class SessionExtensions
    {
        public static void Update(this SessionData source, SessionData updated)
        {
            if (source == null)
                throw new ArgumentNullException("source session is null");
            
            if (updated == null)
                return;

            if (source.ExpectedWord != updated.ExpectedWord)
                source.ExpectedWord = updated.ExpectedWord;
            if (source.Language != updated.Language)
                source.Language = updated.Language;
            if (source.State != updated.State)
                source.State = updated.State;
            if (source.WordSequence != updated.WordSequence)
                source.WordSequence = updated.WordSequence;
        }
    }
}
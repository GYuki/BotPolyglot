using System.Collections.Generic;

namespace LogicBlock.Translations.Model
{
    public class Word
    {
        public int Id { get; set; }

        public List<AbstractLanguage> Translations { get; set; }
    }
}
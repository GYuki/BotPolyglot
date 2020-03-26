namespace LogicBlock.Translations.Model
{
    public abstract class AbstractLanguage
    {
        public int Id { get; set; }
        public string Translation { get; set; }
        public Word Word { get; set; }
        public int WordId { get; set; }
    }

    public class RuLanguage : AbstractLanguage
    {

    }

    public class EnLanguage : AbstractLanguage
    {

    }
}
namespace LogicBlock.Translations.Model
{
    public abstract class AbstractLanguage
    {
        public string Translation { get; set; }
        public int WordId { get; set; }
    }

    public class RuLanguage : AbstractLanguage
    {

    }

    public class EnLanguage : AbstractLanguage
    {
        
    }
}
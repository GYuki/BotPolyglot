namespace LogicBlock.Translations.Model
{
    public interface ILanguage
    {
        int Id { get; set; }
        string Translation { get; set; }
        Word Word { get; set; }
        int WordId { get; set; }
    }
    public abstract class AbstractLanguage : ILanguage
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
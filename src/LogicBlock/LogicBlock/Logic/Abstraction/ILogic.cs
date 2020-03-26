using LogicBlock.Info;

namespace LogicBlock.Logic
{
    public interface ILogic
    {
        void HandleText(ITextRequestInfo info);
    }

    public interface ITutorialLogic : ILogic {}
    public interface IArcadeLogic : ILogic {}
}
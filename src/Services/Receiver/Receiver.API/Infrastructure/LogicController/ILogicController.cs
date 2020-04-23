using LogicBlock.Session;
using Receiver.API.States;

namespace Receiver.API.Infrastructure.LogicController
{
    public interface ILogicController
    {
        ILogic GetLogic(State state);
        IActionLogic GetActionLogic(State state);
    }
}
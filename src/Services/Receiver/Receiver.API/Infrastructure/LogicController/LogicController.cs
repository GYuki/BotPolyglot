
using LogicBlock.Session;
using Receiver.API.States;

namespace Receiver.API.Infrastructure.LogicController
{
    public class LogicController : ILogicController
    {
        private readonly IIdleLogic _idle;
        private readonly ILanguageLogic _language;
        private readonly IModeChooseLogic _mode;
        private readonly IArcadeActionLogic _arcadeAction;
        private readonly ITutorialActionLogic _tutorialAction;

        public LogicController(
            IIdleLogic idle,
            ILanguageLogic language,
            IModeChooseLogic mode,
            IArcadeActionLogic arcadeAction,
            ITutorialActionLogic tutorialAction
        )
        {
            _idle = idle;
            _language = language;
            _mode = mode;
            _arcadeAction = arcadeAction;
            _tutorialAction = tutorialAction;
        }

        public ILogic GetLogic(State state) =>
            state switch
            {
                State.Idle => _idle,
                State.LanguageChoose => _language,
                State.ModeChoose => _mode,
                State.ArcadeAction => _arcadeAction,
                State.TutorialAction => _tutorialAction,
                _ => null
            };

        public IActionLogic GetActionLogic(State state) => GetLogic(state) as IActionLogic;
    }
}
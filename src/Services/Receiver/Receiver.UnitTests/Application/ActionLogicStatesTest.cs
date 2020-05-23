using System.Collections.Generic;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Logic;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model.Texts;
using LogicBlock.Utils;
using Moq;
using NUnit.Framework;
using Receiver.API.States;

namespace UnitTest.Receiver.Application
{
    [TestFixture]
    public class ActionLogicStatesTest
    {
        private readonly Mock<LogicBlock.Logic.IArcadeLogic> _logicMock;
        private readonly Mock<ITranslationsRepository> _logicTranslations;

        public ActionLogicStatesTest()
        {
            _logicMock = new Mock<LogicBlock.Logic.IArcadeLogic>();
            _logicTranslations = new Mock<ITranslationsRepository>();
        }

        [SetUp]
        public void Setup()
        {
            var message = new Text
            {
                Russian = "translation"
            };
            
            _logicTranslations.Setup(x => x.GetText(It.IsAny<string>()))
                .Returns(Task.FromResult(message));
        }

        [Test]
        public async Task Arcade_State_Correct_Word_Success()
        {
            // Arrange
            var fakeState = State.ArcadeAction;
            var fakeLanguage = "language";
            var fakeSequence = new List<int> { 1, 2, 3};

            var fakeSession = GetFakeSession(
                fakeState: fakeState,
                fakeLanguage: fakeLanguage,
                fakeSequence: fakeSequence);
            var fakeMessage = "message";
            var fakeResponse = GetFakeResponse();

            _logicMock.Setup(x => x.HandleText(It.IsAny<ITextRequestInfo>()))
                .Returns(Task.FromResult(fakeResponse));

            // Act
            var logicState = new ArcadeActionLogic(
                _logicMock.Object,
                _logicTranslations.Object
            );

            var actionResult = await logicState.Act(fakeMessage, fakeSession);

            // Assert
            Assert.IsTrue(actionResult.Success);
        }

        [Test]
        public async Task Arcade_State_Null_Sequence()
        {
            // Arrange
            var fakeState = State.ArcadeAction;
            var fakeLanguage = "language";

            var fakeSession = GetFakeSession(
                fakeState: fakeState,
                fakeLanguage: fakeLanguage);
            var fakeMessage = "message";
            var fakeResponse = GetFakeStartResponse();

            _logicMock.Setup(x => x.StartChat(It.IsAny<IStartRequestInfo>()))
                .Returns(Task.FromResult(fakeResponse));

            // Act
            var logic = new ArcadeActionLogic(
                _logicMock.Object,
                _logicTranslations.Object
            );

            var actionResult = await logic.Act(fakeMessage, fakeSession);

            // Assert
            Assert.IsTrue(actionResult.Success);
        }

        [Test]
        public void Arcade_State_Quit()
        {
            // Arrange
            var fakeState = State.ArcadeAction;
            var fakeLanguage = "language";
            var fakeSequence = new List<int> { 1, 2, 3};

            var fakeSession = GetFakeSession(
                fakeState: fakeState,
                fakeLanguage: fakeLanguage,
                fakeSequence: fakeSequence);
            
            // Act
            var logic = new ArcadeActionLogic(
                _logicMock.Object,
                _logicTranslations.Object
            );

            var actionResult = logic.Menu(fakeSession);

            // Arrange
            Assert.AreEqual(fakeSession.State, State.Idle);
            Assert.IsNull(fakeSession.WordSequence);
            Assert.AreEqual(fakeSession.ExpectedWord, 0);
        }

        private ChatSession GetFakeSession(
            int fakeWord = default,
            List<int> fakeSequence = default,
            State fakeState = default,
            string fakeLanguage = default
            )
        {
            return new ChatSession
            {
                ExpectedWord = fakeWord,
                Language = fakeLanguage,
                State = fakeState,
                WordSequence = fakeSequence
            };
        }

        private IResponseInfo GetFakeResponse()
        {
            return new ArcadeResponseInfo("message", ResponseCodes.OK);
        }

        private IResponseInfo GetFakeStartResponse()
        {
            return new StartResponseInfo(ResponseCodes.OK, "response");
        }
    }
}
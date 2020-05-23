using System.Collections.Generic;
using System.Threading.Tasks;
using LogicBlock.Session;
using LogicBlock.Translations.Infrastructure.Repositories;
using LogicBlock.Translations.Model.Texts;
using Moq;
using NUnit.Framework;
using Receiver.API.States;

namespace UnitTest.Receiver.Application
{
    [TestFixture]
    public class BaseLogicStatesTest
    { 
        private readonly Mock<ITranslationsRepository> _logicTranslationsMock;

        public BaseLogicStatesTest()
        {
            _logicTranslationsMock = new Mock<ITranslationsRepository>();
        }

        [SetUp]
        public void Setup()
        {
            var message = new Text
            {
                Russian = "translation"
            };

            _logicTranslationsMock.Setup(x => x.GetText(It.IsAny<string>()))
                .Returns(Task.FromResult(message));
        }

        [Test]
        public async Task Idle_State_Language_Command()
        {
            // Arrange
            var fakeState = State.Idle;
            var fakeSession = GetFakeSession(fakeState: fakeState);
            var fakeCommand = "language";

            // Act
            var idleLogic = new IdleLogic(_logicTranslationsMock.Object);
            var result = await idleLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(result.Session.State, State.LanguageChoose);
        }

        [Test]
        public async Task Idle_State_Language_Help()
        {
            // Arrange
            var fakeState = State.Idle;
            var fakeSession = GetFakeSession(fakeState: fakeState);
            var fakeCommand = "help";

            // Act
            var idleLogic = new IdleLogic(_logicTranslationsMock.Object);
            var result = await idleLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(result.Session.State, State.Idle);
        }

        [Test]
        public void Idle_State_Back_Command()
        {
            // Arrange
            var fakeState = State.Idle;
            var fakeSession = GetFakeSession(fakeState: fakeState);

            // Act
            var idleLogic = new IdleLogic(_logicTranslationsMock.Object);
            var result = idleLogic.Back(fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.Idle);
        }

        [Test]
        public async Task Idle_State_Incorrect_Command()
        {
            // Arrange
            var fakeState = State.Idle;
            var fakeSession = GetFakeSession(fakeState: fakeState);
            var fakeCommand = "command";

            // Act
            var idleLogic = new IdleLogic(_logicTranslationsMock.Object);
            var result = await idleLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(result.Session.State, State.Idle);
        }

        [Test]
        public async Task Language_State_Command_Exists()
        {
            // Arrange
            var fakeState = State.LanguageChoose;
            var fakeSession = GetFakeSession(fakeState: fakeState);
            var fakeCommand = "en";

            // Act
            var languageLogic = new LanguageChooseLogic(_logicTranslationsMock.Object);
            var result = await languageLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.ModeChoose);
            Assert.AreEqual(fakeSession.Language, fakeCommand);
        }

        [Test]
        public async Task Language_State_Command_Does_Not_Exist()
        {
            // Arrange
            var fakeState = State.LanguageChoose;
            var fakeSession = GetFakeSession(fakeState: fakeState);
            var fakeCommand = "command";

            // Act
            var languageLogic = new LanguageChooseLogic(_logicTranslationsMock.Object);
            var result = await languageLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, fakeState);
            Assert.IsNull(fakeSession.Language);
        }

        [Test]
        public void Language_State_Back_Command()
        {
            // Arrange
            var fakeState = State.LanguageChoose;
            var fakeSession = GetFakeSession(fakeState: fakeState);

            // Act
            var languageLogic = new LanguageChooseLogic(_logicTranslationsMock.Object);
            var result = languageLogic.Back(fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.Idle);
        }

        [Test]
        public async Task ChooseMode_State_Arcade()
        {
            // Arrange
            var fakeState = State.ModeChoose;
            var fakeLanguage = "language";
            var fakeSession = GetFakeSession(fakeState: fakeState, fakeLanguage: fakeLanguage);
            var fakeCommand = "arcade";

            // Act
            var modeLogic = new ModeChooseLogic(_logicTranslationsMock.Object);
            var result = await modeLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.ArcadeAction);
        }

        [Test]
        public async Task ChooseMode_State_Tutorial()
        {
            // Arrange
            var fakeState = State.ModeChoose;
            var fakeLanguage = "language";
            var fakeSession = GetFakeSession(fakeState: fakeState, fakeLanguage: fakeLanguage);
            var fakeCommand = "tutorial";

            // Act
            var modeLogic = new ModeChooseLogic(_logicTranslationsMock.Object);
            var result = await modeLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.TutorialAction);
        }

        [Test]
        public async Task ChooseMode_State_Incorrect_Command()
        {
            // Arrange
            var fakeState = State.ModeChoose;
            var fakeLanguage = "language";
            var fakeSession = GetFakeSession(fakeState: fakeState, fakeLanguage: fakeLanguage);
            var fakeCommand = "command";

            // Act
            var modeLogic = new ModeChooseLogic(_logicTranslationsMock.Object);
            var result = await modeLogic.Act(fakeCommand, fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.ModeChoose);
        }

        [Test]
        public void ChooseMode_State_Back()
        {
            // Arrange
            var fakeState = State.ModeChoose;
            var fakeLanguage = "language";
            var fakeSession = GetFakeSession(fakeState: fakeState, fakeLanguage: fakeLanguage);

            // Act
            var modeLogic = new ModeChooseLogic(_logicTranslationsMock.Object);
            var result = modeLogic.Back(fakeSession);

            // Assert
            Assert.AreEqual(fakeSession.State, State.LanguageChoose);
            Assert.IsNull(fakeSession.Language);
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
    }
}
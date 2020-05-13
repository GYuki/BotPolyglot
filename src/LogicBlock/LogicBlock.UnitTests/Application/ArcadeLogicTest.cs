using NUnit.Framework;
using Moq;
using LogicBlock.Translations.Infrastructure.Repositories;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Session;
using LogicBlock.Translations.Model;
using System.Collections.Generic;
using LogicBlock.Logic;
using LogicBlock.Utils;
using LogicBlock.Translations.Model.Texts;

namespace UnitTest.LogicBlock.Application
{
    [TestFixture]
    public class ArcadeLogicTest
    {
        private readonly Mock<ILanguageRepository> _languageRepositoryMock;
        private readonly Mock<ITranslationsRepository> _translationsRepositoryMock;

        public ArcadeLogicTest()
        {
            _languageRepositoryMock = new Mock<ILanguageRepository>();
            _translationsRepositoryMock = new Mock<ITranslationsRepository>();
        }

        [SetUp]
        public void Setup()
        {
            var fakeTranslation = new Text
            {
                Russian = "translation"
            };

            _translationsRepositoryMock.Setup(x => x.GetText(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeTranslation));
        }

        [Test]
        public async Task Handle_Text_With_No_Translation_Should_Return_False()
        {
            // Arrange
            var fakeWord = 0;
            var fakeMessage = "fake";
            var fakeSequence = new List<int> { 1, 2 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);

            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<ILanguage>()));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );

            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.NoAsnwers);
        }

        [Test]
        public async Task Handle_Text_Success()
        {
            // Arrange
            var fakeWord = 0;
            var fakeMessage = "fake";
            var fakeSequence = new List<int> { 1, 2 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);
            var fakeTranslations = CreateFakeTranslations(fakeMessage);
            
            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeTranslations));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );
            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.OK);
        }

        [Test]
        public async Task Handle_Text_Wrong_Message()
        {
            var fakeWord = 0;
            var fakeMessage = "fake";
            var fakeSequence = new List<int>{ 1, 2 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);
            var fakeTranslations = CreateFakeTranslations("wrong");

            int lastKnownWordId = fakeRequest.Request.Session.ExpectedWord;

            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeTranslations));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );
            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.WrongAnswer);
            Assert.AreNotEqual(fakeRequest.Request.Session.ExpectedWord, lastKnownWordId);
        }

        [Test]
        public async Task Handle_Text_Wrong_Message_Last_Word()
        {
            var fakeWord = 1;
            var fakeMessage = "fake";
            var fakeSequence = new List<int>{ 1, 2 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);
            var fakeTranslations = CreateFakeTranslations("wrong");

            int lastKnownWordId = fakeRequest.Request.Session.ExpectedWord;

            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeTranslations));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );
            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.WrongAnswer);
            Assert.AreEqual(fakeRequest.Request.Session.ExpectedWord, 0);
            Assert.IsNull(fakeRequest.Request.Session.WordSequence);
        }

        [Test]
        public async Task Handle_Text_Empty_Word_Sequence()
        {
            // Arrange
            var fakeWord = 0;
            var fakeMessage = "fake";
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage);

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );

            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.LogicInternalError);
        }

        [Test]
        public async Task Handle_Text_Last_Word_In_Sequence()
        {
            // Arrange
            var fakeWord = 2;
            var fakeMessage = "fake";
            var fakeSequence = new List<int> { 1, 2, 3 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);
            var lastKnownSequence = new List<int>(fakeSequence);

            var fakeTranslations = CreateFakeTranslations(fakeMessage);

            _languageRepositoryMock.Setup(x => x.GetWordsCountAsync())
                .Returns(Task.FromResult(4));
            
            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeTranslations));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );

            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.OK);
            Assert.AreNotEqual(lastKnownSequence, fakeRequest.Request.Session.WordSequence);
            Assert.AreEqual(fakeRequest.Request.Session.ExpectedWord, 0);
        }

        [Test]
        public async Task Handle_Text_Word_Index_More_Than_Sequence_Count()
        {
            // Arrange
            var fakeWord = 2;
            var fakeMessage = "fake";
            var fakeSequence = new List<int> { 1, 2 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);
            var lastKnownSequence = new List<int>(fakeSequence);            
            
            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );

            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.LogicInternalError);
        }

        [Test]
        public async Task Handle_Text_After_Action()
        {
            // Arrange
            var fakeWord = 0;
            var fakeRequest = CreateAfterActionRequest(fakeWord);

            _languageRepositoryMock.Setup(x => x.GetWordsCountAsync())
                .Returns(Task.FromResult(4));
            _languageRepositoryMock.Setup(x => x.GetNextTask(It.IsAny<int>()))
                .Returns(Task.FromResult("test"));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object,
                _translationsRepositoryMock.Object
            );

            var result = await arcadeLogic.AfterAction(fakeRequest);

            // Assert
            Assert.AreEqual(result.ResponseCode, ResponseCodes.OK);
            Assert.NotNull(fakeRequest.Request.Session.WordSequence);
            Assert.AreEqual(fakeRequest.Request.Session.ExpectedWord, 0);
        }

        private TextRequestInfo CreateFakeTextRequest(int fakeWord, string fakeMessage, List<int> fakeSequence=default)
        {
            return new TextRequestInfo
            {
                Request = new TextRequest(
                    new ChatSession
                    {
                        ExpectedWord = fakeWord,
                        WordSequence = fakeSequence
                    },
                    fakeMessage
                )
            };
        }

        private AfterActionRequestInfo CreateAfterActionRequest(int fakeWord, List<int> fakeSequence=default)
        {
            return new AfterActionRequestInfo
            {
                Request = new AfterActionRequest(
                    new ChatSession
                    {
                        ExpectedWord = fakeWord,
                        WordSequence = fakeSequence
                    }
                )
            };
        }
    
        private List<ILanguage> CreateFakeTranslations(string fakeWord)
        {
            var fakeTranslation = new EnLanguage();
            fakeTranslation.Translation = fakeWord;
            fakeTranslation.Word = new Word();
            return new List<ILanguage>
            {
                fakeTranslation
            };
        }
    } 
}
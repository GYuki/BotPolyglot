using NUnit.Framework;
using Moq;
using LogicBlock.Translations.Infrastructure.Repositories;
using System.Threading.Tasks;
using LogicBlock.Info;
using LogicBlock.Session;
using LogicBlock.Translations.Model;
using System.Collections.Generic;
using LogicBlock.Logic;

namespace UnitTest.LogicBlock.Application
{
    [TestFixture]
    public class ArcadeLogicTest
    {
        private readonly Mock<ILanguageRepository> _languageRepositoryMock;

        public ArcadeLogicTest()
        {
            _languageRepositoryMock = new Mock<ILanguageRepository>();
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
                _languageRepositoryMock.Object
            );

            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.IsFalse(result.Success);
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
                _languageRepositoryMock.Object
            );
            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.IsTrue(result.Success);
        }

        [Test]
        public async Task Handle_Text_Wrong_Message()
        {
            var fakeWord = 0;
            var fakeMessage = "fake";
            var fakeSequence = new List<int>{ 1, 2 };
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage, fakeSequence);
            var fakeTranslations = CreateFakeTranslations("wrong");

            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeTranslations));

            // Act
            var arcadeLogic = new ArcadeLogic(
                _languageRepositoryMock.Object
            );
            var result = await arcadeLogic.HandleText(fakeRequest);

            // Assert
            Assert.IsFalse(result.Success);
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
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
        private readonly Mock<ILanguageRepository<AbstractLanguage>> _languageRepositoryMock;

        public ArcadeLogicTest()
        {
            _languageRepositoryMock = new Mock<ILanguageRepository<AbstractLanguage>>();
        }

        [Test]
        public async Task Handle_Text_With_No_Translation_Should_Return_False()
        {
            // Arrange
            var fakeWord = 0;
            var fakeMessage = "fake";
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage);

            _languageRepositoryMock.Setup(x => x.GetWordTranslationsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new List<AbstractLanguage>()));

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
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage);
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
            var fakeRequest = CreateFakeTextRequest(fakeWord, fakeMessage);
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

        private TextRequestInfo CreateFakeTextRequest(int fakeWord, string fakeMessage)
        {
            return new TextRequestInfo
            {
                Request = new TextRequest(
                    new Session
                    {
                        ExpectedWord = fakeWord
                    },
                    fakeMessage
                )
            };
        }
    
        private List<AbstractLanguage> CreateFakeTranslations(string fakeWord)
        {
            var fakeTranslation = new Mock<AbstractLanguage>().Object;
            fakeTranslation.Translation = fakeWord;
            return new List<AbstractLanguage>
            {
                fakeTranslation
            };
        }
    } 
}
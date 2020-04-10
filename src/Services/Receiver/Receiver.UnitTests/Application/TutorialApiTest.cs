using NUnit.Framework;
using Moq;
using LogicBlock.Logic;
using System.Threading.Tasks;
using LogicBlock.Session;
using LogicBlock.Info;
using Receiver.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Receiver.API.Models;

namespace UnitTest.Receiver.Application
{
    [TestFixture]
    public class TutorialApiTest
    {
        private readonly Mock<ITutorialLogic> _tutorialMock;

        public TutorialApiTest()
        {
            _tutorialMock = new Mock<ITutorialLogic>();
        }

        [Test]
        public async Task Handle_Start_Success()
        {
            // Arrange
            var fakeSession = MakeFakeSession();

            var fakeMessage = "fake";
            var fakeSuccess = true;
            var fakeResponse = MakeFakeResponse(fakeMessage, fakeSuccess);

            _tutorialMock.Setup(x => x.StartChat(It.IsAny<IStartRequestInfo>()))
                .Returns(Task.FromResult((IResponseInfo)fakeResponse));
            
            // Act
            var tutorialController = new TutorialController(
                _tutorialMock.Object
            );

            var result = await tutorialController.HandleStartAsync(fakeSession);

            // Assert
            Assert.AreEqual((result.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)result.Result).Value as ResponseModel).Message, fakeResponse.Message);
            Assert.AreEqual((((ObjectResult)result.Result).Value as ResponseModel).Success, fakeResponse.Success);
        }

        [Test]
        public async Task Handle_Start_Null_Session_Should_Return_Bad_Request()
        {
            // Arrange
            ChatSession fakeSession = null;

            var fakeMessage = "fake";
            var fakeSuccess = true;
            var fakeResponse = MakeFakeResponse(fakeMessage, fakeSuccess);

            _tutorialMock.Setup(x => x.StartChat(It.IsAny<IStartRequestInfo>()));

            // Act
            var tutorialController = new TutorialController(
                _tutorialMock.Object
            );

            var result = (await tutorialController.HandleStartAsync(fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Handle_Action_Success()
        {
            // Arrange
            ChatSession fakeSession = MakeFakeSession();
            
            var fakeMessage = "message";

            var fakeResponseMessage = "fake";
            var fakeSuccess = true;
            var fakeResponse = MakeFakeResponse(fakeResponseMessage, fakeSuccess);

            _tutorialMock.Setup(x => x.HandleText(It.IsAny<ITextRequestInfo>()))
                .Returns(Task.FromResult((IResponseInfo)fakeResponse));
            
            // Act
            var tutorialController = new TutorialController(
                _tutorialMock.Object
            );

            var result = await tutorialController.HandleActionAsync(fakeMessage, fakeSession);

            // Assert
            Assert.AreEqual((result.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)result.Result).Value as ResponseModel).Message, fakeResponse.Message);
            Assert.AreEqual((((ObjectResult)result.Result).Value as ResponseModel).Success, fakeResponse.Success);
        }

        [Test]
        public async Task Handle_Action_Null_Session_Should_Return_Bad_Request()
        {
            ChatSession fakeSession = null;

            var fakeMessage = "message";

            var fakeResponseMessage = "fake";
            var fakeSuccess = true;
            var fakeResponse = MakeFakeResponse(fakeResponseMessage, fakeSuccess);

            _tutorialMock.Setup(x => x.HandleText(It.IsAny<ITextRequestInfo>()));

            // Act
            var tutorialController = new TutorialController(
                _tutorialMock.Object
            );

            var result = (await tutorialController.HandleActionAsync(fakeMessage, fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
        }

        private ChatSession MakeFakeSession()
        {
            return new ChatSession();
        }

        private StartResponseInfo MakeFakeResponse(string fakeMessage, bool fakeSuccess)
        {
            return new StartResponseInfo(fakeSuccess, fakeMessage);
        }
    }
}
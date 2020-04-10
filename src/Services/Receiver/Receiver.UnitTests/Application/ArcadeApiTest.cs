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
    public class ArcadeApiTest
    {
        private readonly Mock<IArcadeLogic> _logicMock;

        public ArcadeApiTest()
        {
            _logicMock = new Mock<IArcadeLogic>();
        }

        [Test]
        public async Task Handle_Start_Success()
        {
            // Arrange
            var fakeSession = MakeFakeSession();

            var fakeMessage = "fake";
            var fakeSuccess = true;
            var fakeResponse = MakeFakeResponse(fakeMessage, fakeSuccess);

            _logicMock.Setup(x => x.StartChat(It.IsAny<IStartRequestInfo>()))
                .Returns(Task.FromResult((IResponseInfo)fakeResponse));
            
            // Act
            var arcadeController = new ArcadeController(
                _logicMock.Object
            );

            var result = await arcadeController.HandleStartAsync(fakeSession);

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

            _logicMock.Setup(x => x.StartChat(It.IsAny<IStartRequestInfo>()));

            // Act
            var arcadeController = new ArcadeController(
                _logicMock.Object
            );

            var result = (await arcadeController.HandleStartAsync(fakeSession)).Result as BadRequestResult;

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

            _logicMock.Setup(x => x.HandleText(It.IsAny<ITextRequestInfo>()))
                .Returns(Task.FromResult((IResponseInfo)fakeResponse));
            
            // Act
            var arcadeController = new ArcadeController(
                _logicMock.Object
            );

            var result = await arcadeController.HandleActionAsync(fakeMessage, fakeSession);

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

            _logicMock.Setup(x => x.HandleText(It.IsAny<ITextRequestInfo>()));

            // Act
            var arcadeController = new ArcadeController(
                _logicMock.Object
            );

            var result = (await arcadeController.HandleActionAsync(fakeMessage, fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.IsNotNull(result);
        }

        private ChatSession MakeFakeSession()
        {
            return new ChatSession();
        }

        private StartRequestInfo MakeFakeStartRequest(ChatSession fakeSession)
        {
            return new StartRequestInfo
            {
                OperationRequest = new StartRequest(fakeSession)
            };
        }

        private StartResponseInfo MakeFakeResponse(string fakeMessage, bool fakeSuccess)
        {
            return new StartResponseInfo(fakeSuccess, fakeMessage);
        }
    }
}
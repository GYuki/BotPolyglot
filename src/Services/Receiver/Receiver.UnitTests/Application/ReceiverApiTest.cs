using NUnit.Framework;
using Moq;
using Receiver.API.Infrastructure.LogicController;
using System.Threading.Tasks;
using LogicBlock.Session;
using Receiver.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Receiver.API.States;

namespace UnitTest.Receiver.Application
{
    [TestFixture]
    public class ReceiverApiTest
    {
        private readonly Mock<ILogicController> _logicControllerMock;

        public ReceiverApiTest()
        {
            _logicControllerMock = new Mock<ILogicController>();
        }

        [Test]
        public async Task Handle_Logic_Empty_Request_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeMessage = "";
            ChatSession fakeSession = null;

            _logicControllerMock.Setup(x => x.GetLogic(It.IsAny<State>()));

            // Act
            var receiverController = new ReceiverController(
                _logicControllerMock.Object
            );

            var actionResult = (await receiverController.HandleLogicAsync(fakeMessage, fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Handle_After_Action_Empty_Request_Should_Return_Bad_Request()
        {
            // Arrange
            ChatSession fakeSession = null;

            _logicControllerMock.Setup(x => x.GetActionLogic(It.IsAny<State>()));

            // Act
            var receiverController = new ReceiverController(
                _logicControllerMock.Object
            );

            var actionResult = (await receiverController.HandleAfterActionAsync(fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Handle_Logic_Wrong_State_Should_Return_Bad_Request()
        {
            // Arrange
            var fakeMessage = "fake";
            int fakeState = -1;

            ChatSession fakeSession = GetFakeSession((State)fakeState);
            ILogic fakeLogic = null;

            _logicControllerMock.Setup(x => x.GetLogic(It.IsAny<State>()))
                .Returns(fakeLogic);
            
            // Act
            var receiverController = new ReceiverController(
                _logicControllerMock.Object
            );

            var actionResult = (await receiverController.HandleLogicAsync(fakeMessage, fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Handle_After_Action_Wrong_State_Should_Return_Bad_Request()
        {
            // Arrange
            int fakeState = -1;

            ChatSession fakeSession = GetFakeSession((State)fakeState);
            IActionLogic fakeLogic = null;

            _logicControllerMock.Setup(x => x.GetActionLogic(It.IsAny<State>()))
                .Returns(fakeLogic);
            
            // Act
            var receiverController = new ReceiverController(
                _logicControllerMock.Object
            );

            var actionResult = (await receiverController.HandleAfterActionAsync(fakeSession)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Handle_Logic_Success()
        {
            // Arrange
            var fakeMessage = "fake";
            int fakeState = 0;

            ChatSession fakeSession = GetFakeSession((State)fakeState);
            ILogic fakeLogic = GetFakeLogic();

            _logicControllerMock.Setup(x => x.GetLogic(It.IsAny<State>()))
                .Returns(fakeLogic);
            // Act
            var receiverController = new ReceiverController(
                _logicControllerMock.Object
            );

            var actionResult = await receiverController.HandleLogicAsync(fakeMessage, fakeSession);

            //  Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        private ChatSession GetFakeSession(State fakeState)
        {
            return new ChatSession
            {
                State = fakeState
            };
        }

        private ILogic GetFakeLogic()
        {
            return new IdleLogic();
        }
    }
}
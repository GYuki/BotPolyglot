using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Session.API.Controllers;
using Session.API.Infrastructure.Repositories;
using Session.API.Model;

namespace UnitTest.Session.Application
{
    [TestFixture]
    public class SessionApiTest
    {
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;

        public SessionApiTest()
        {
            _sessionRepositoryMock = new Mock<ISessionRepository>();
        }

        [Test]
        public async Task Get_Session_Success_Should_Return_Ok()
        {
            // Arrange
            var fakeAuth = 0;
            long fakeChatId = 1;
            var fakeSession = GetFakeSession(fakeAuth, fakeChatId);

            _sessionRepositoryMock.Setup(x => x.GetSessionAsync(It.IsAny<long>(), It.IsAny<AuthType>()))
                .Returns(Task.FromResult(fakeSession));
            
            // Act
            var sessionController = new SessionController(
                _sessionRepositoryMock.Object
            );

            var actionResult = await sessionController.GetSessionAsync(fakeAuth, fakeChatId);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.AreEqual((((ObjectResult)actionResult.Result).Value as SessionModel).ChatId, fakeChatId);
        }

        [Test]
        public async Task Get_Session_Not_Found()
        {
            // Arrange
            var fakeAuth = 0;
            long fakeChatId = 1;
            SessionModel fakeSession = null;

            _sessionRepositoryMock.Setup(x => x.GetSessionAsync(It.IsAny<long>(), It.IsAny<AuthType>()))
                .Returns(Task.FromResult(fakeSession));
            
            // Act
            var sessionController = new SessionController(
                _sessionRepositoryMock.Object
            );

            var actionResult = (await sessionController.GetSessionAsync(fakeAuth, fakeChatId)).Result as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Get_Session_Bad_Request()
        {
            // Arrange
            var fakeAuth = 0;
            long fakeChatId = 0;

            _sessionRepositoryMock.Setup(x => x.GetSessionAsync(It.IsAny<long>(), It.IsAny<AuthType>()));

            // Act
            var sessionController = new SessionController(
                _sessionRepositoryMock.Object
            );

            var actionResult = (await sessionController.GetSessionAsync(fakeAuth, fakeChatId)).Result as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Test]
        public async Task Update_Session_Returns_Ok()
        {
            // Arrange
            var fakeAuth = 0;
            long fakeChatId = 1;
            var fakeSession = GetFakeSession(fakeAuth, fakeChatId);

            _sessionRepositoryMock.Setup(x => x.UpdateSessionAsync(It.IsAny<SessionModel>()))
                .Returns(Task.FromResult(fakeSession));
            
            // Act
            var sessionController = new SessionController(
                _sessionRepositoryMock.Object
            );

            var actionResult = await sessionController.UpdateOrCreateSessionAsync(fakeSession);

            // Assert
            Assert.AreEqual((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        private SessionModel GetFakeSession(int fakeAuth, long chatId)
        {
            return new SessionModel
            {
                AuthType = (AuthType)fakeAuth,
                ChatId = chatId
            };
        }
    }
}
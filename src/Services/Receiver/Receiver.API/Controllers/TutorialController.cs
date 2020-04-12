using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net;
using LogicBlock.Logic;
using LogicBlock.Session;
using LogicBlock.Info;
using Receiver.API.Models;

namespace Receiver.API.Controllers
{
    [Route("receiver/[controller]")]
    [ApiController]
    public class TutorialController : Controller
    {
        private readonly ITutorialLogic _tutorial;

        public TutorialController(ITutorialLogic tutorial)
        {
            _tutorial = tutorial;
        }

        [HttpPost]
        [Route("action/{message}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseModel>> HandleActionAsync(string message, [FromBody]ChatSession session)
        {
            if (string.IsNullOrEmpty(message) || session == null)
                return BadRequest();
            
            TextRequestInfo info = new TextRequestInfo
            {
                Request = new TextRequest(session, message)
            };

            var result = await _tutorial.HandleText(info);

            var response = new ResponseModel
            {
                Message = result.Message,
                Session = info.Request.Session,
                Success = result.Success
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("start")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseModel>> HandleStartAsync([FromBody]ChatSession session)
        {
            if (session == null)
                return BadRequest();

            StartRequestInfo info = new StartRequestInfo
            {
                OperationRequest = new StartRequest(session)
            };

            var result = await _tutorial.StartChat(info);

            var response = new ResponseModel
            {
                Message = result.Message,
                Session = info.Request.Session,
                Success = result.Success
            };

            return Ok(response);
        }
    }
}
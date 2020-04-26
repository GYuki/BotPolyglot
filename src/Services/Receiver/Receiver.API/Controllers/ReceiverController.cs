using LogicBlock.Session;
using Microsoft.AspNetCore.Mvc;
using Receiver.API.Infrastructure.LogicController;
using Receiver.API.Models;
using System.Net;
using System.Threading.Tasks;

namespace Receiver.API.Controllers
{
    [Route("api/[controller]")]
    public class ReceiverController : Controller
    {
        private readonly ILogicController _logic;

        public ReceiverController(ILogicController logic)
        {
            _logic = logic;
        }

        [HttpPost]
        [Route("action/{message}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseModel>> HandleLogicAsync(string message, [FromBody]ChatSession session)
        {
            if (string.IsNullOrEmpty(message) || session == null)
                return BadRequest();
            
            var currentState = _logic.GetLogic(session.State);

            if (currentState == null)
                return BadRequest();

            var result = new ResponseModel()
            {
                Session = session
            };

            switch(message)
            {
                case "back":
                    result.Message = currentState.Back(session);
                    break;
                case "menu":
                    result.Message = currentState.Menu(session);
                    break;
                default:
                    result = await currentState.Act(message, session);
                    break;
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("afteraction/")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> HandleAfterActionAsync([FromBody]ChatSession session)
        {
            if (session == null)
                return BadRequest();
            
            var currentState = _logic.GetActionLogic(session.State);

            if (currentState == null)
                return BadRequest();
            
            var result = await currentState.GetNextTask(session);

            return Ok(result);
        }
    }
}
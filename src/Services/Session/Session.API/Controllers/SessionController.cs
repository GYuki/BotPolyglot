using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Session.API.Infrastructure.Repositories;
using Session.API.Model;

namespace Session.API.Controllers
{
    [Route("api/[controller]")]
    public class SessionController : Controller
    {
        private readonly ISessionRepository _session;
        public SessionController(ISessionRepository session)
        {
            _session = session;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(SessionModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SessionModel>> GetSessionAsync([FromQuery]int authType, [FromQuery]long chatId)
        {
            if (chatId == 0)
                return BadRequest();
            
            var result = await _session.GetSessionAsync(chatId, (AuthType)authType);

            if (result == null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(SessionModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SessionModel>> UpdateOrCreateSessionAsync([FromBody]SessionModel model)
        {
            if (model == null)
                return BadRequest();
            
            var result = await _session.UpdateSessionAsync(model);

            if (result == null)
                return Conflict();
            
            return Ok(result);
        }
    }
}
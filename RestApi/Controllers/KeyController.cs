using KeyMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GateKeeper.Controllers
{
    /**
     * Used for commands related to a standard key up/down event
     * */
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class KeyController : ControllerBase
    {
        /**
         * Client will send in a specific key command along with modifiers and the event type - key down or key up
         * */
        [HttpPost]
        public IActionResult Post([FromBody] Command value)
        {
            bool result = Action.SendCommand(value, false);
            if (result)
                return Ok(new { Consumes = "application/json", Values = value });
            else
                return Problem("error processing command");
        }
    }
}

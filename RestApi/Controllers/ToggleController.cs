using KeyMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GateKeeper.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ToggleController : ControllerBase
    {
        /**
         * Client will send in a specific key command along with modifiers and the event type.  
         * The server will process both the key down and up commands.
         * */
        [HttpPost]
        public void Post(Command value)
        {
            Action.SendCommand(value, true);
        }
    }
}

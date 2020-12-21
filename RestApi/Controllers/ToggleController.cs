using GIC.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GIC.RestApi.Controllers
{
    /**
     * Used for commands related to a single key stroke event
     * */
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ToggleController : AbstractBase
    {
        /**
         * Client will send in a specific key command along with modifiers and the event type.  
         * The server will process both the key down and up commands.
         * */
        [HttpPost]
        public IActionResult Post([FromBody] Command value)
        {
            Program.ReceivedKey(value, true);
            return SendKeystroke(value, true);
        }
    }
}

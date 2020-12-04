using GIC.KeyMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GIC.RestApi.Controllers
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
            Console.Write($"Received {value.Key} {value.Modifier} {value.activatorType}");
            bool result = KeyMaster.Action.SendCommand(value, false);
            if (result)
            {
                Console.WriteLine(" OK");
                return Ok(new { Consumes = "application/json", Values = value });
            }
            else
            {
                Console.WriteLine(" Failed");
                return Problem("error processing command");
            }
        }
    }
}

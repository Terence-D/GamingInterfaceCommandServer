using GIC.KeyMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GIC.RestApi.Controllers
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
        public IActionResult Post(Command value)
        {
            Console.Write($"Received {value.Key} {value.Modifier} {value.activatorType}");
            bool result = KeyMaster.Action.SendCommand(value, true);
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

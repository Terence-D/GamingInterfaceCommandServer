﻿using GIC.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GIC.RestApi.Controllers
{
    /**
     * Used for commands related to a standard key up/down event
     * */
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class KeyController : AbstractBase
    {
        /**
        * Client will send in a specific key command along with modifiers and the event type.
        * Server will process either key down or key up
        * */
        [HttpPost]
        public IActionResult Post([FromBody] Command value)
        {
            Program.ReceivedKey(value, false);
            return SendKeystroke(value, false);
        }
    }
}

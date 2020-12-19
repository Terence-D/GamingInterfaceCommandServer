using GIC.Common;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GIC.RestApi.Controllers
{
    public class AbstractBase : ControllerBase
    {
        protected IActionResult SendKeystroke(Command command, bool toggle)
        {
            Console.WriteLine($" Received {command.Key} Toggle={toggle.ToString().ToUpper()}");
            bool result = KeyMaster.Action.SendCommand(command.ActivatorType, command.Key, command.Modifier, toggle);
            if (result)
            {
                Console.WriteLine(" OK");
                return Ok(new { Consumes = "application/json", Values = command });
            }
            else
            {
                Console.WriteLine(" Failed");
                return Problem("error processing command");
            }
        }
    }
}

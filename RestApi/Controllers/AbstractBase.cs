using GIC.KeyMaster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIC.RestApi.Controllers
{
    public class AbstractBase : ControllerBase
    {
        protected IActionResult SendKeystroke(Command command, bool toggle)
        {
            Console.WriteLine($" Received {command.Key} Toggle={toggle.ToString().ToUpper()}");
            bool result = KeyMaster.Action.SendCommand(command, toggle);
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

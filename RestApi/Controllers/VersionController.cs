using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GIC.RestApi.Controllers
{
    /**
     * Used to validate the version of the server is what the client expects
     * */
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private const string API_VERSION = "2.0.0.0";
        /**
         * Returns current API version of this server
         * */
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Get()
        {
            Program.ReceivedVersionCheck();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = API_VERSION;

            return Ok(new { Version = version });
        }
    }
}

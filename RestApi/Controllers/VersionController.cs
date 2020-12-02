using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GIC.RestApi.Controllers
{
    /**
     * Used to validate the version of the server is what the client expects
     * */
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private const string API_VERSION = "2.0.0.0";
        /**
         * Returns current API version of this server
         * */
        [HttpPost]
        public IActionResult Get()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            //FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = API_VERSION;// fileVersionInfo.ProductVersion;

            return Ok(new { Consumes = "application/json", Values = version });
        }
    }
}

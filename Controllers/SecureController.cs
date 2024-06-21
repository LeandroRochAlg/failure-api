using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace failure_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("check")]
        public IActionResult Get()
        {
            return Ok("You are authorized to access this resource.");
        }
    }
}
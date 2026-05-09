using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SMS.API.Controllers
{
    [ApiController]
    [Route("api/AuditLog")]
    public class AuditLogController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}

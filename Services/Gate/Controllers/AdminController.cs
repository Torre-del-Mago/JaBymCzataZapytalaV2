using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/admin/")]
    public class AdminController : ControllerBase
    {
        public AdminController() { 
        }

        [HttpGet("admin-info")]
        public async Task<IActionResult> getAdminInfo()
        {
            throw new NotImplementedException();
        }
    }
}
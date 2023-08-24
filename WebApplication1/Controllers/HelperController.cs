using Microsoft.AspNetCore.Mvc;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelperController : ControllerBase
    {
        private CheckHelper _checkHelper;
        public HelperController(CheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        [HttpGet(Name = "IsConnectedToInternet")]
        public IActionResult IsConnectedToInternet()
        {
            var call = _checkHelper.CheckConnectionJobAsync();

            bool isConnected = false;
            isConnected = _checkHelper.IsConnectedToInternet();
            if (isConnected)
            {
                return new JsonResult(new {Status="Connected"});
            }
            return new JsonResult(new { Status = "NotConnected" });
        }

    }
}

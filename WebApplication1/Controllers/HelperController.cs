using Microsoft.AspNetCore.Mvc;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelperController : ControllerBase
    {
        private  CheckHelper _checkHelper;
        public HelperController(CheckHelper checkHelper)
        {
            _checkHelper = checkHelper;   
        }
      
        [HttpGet(Name = "IsConnectedToInternet")]
        public IActionResult IsConnectedToInternet()
        {
            bool isConnected = false;
            isConnected= _checkHelper.IsConnectedToInternet();
            if(isConnected)
            {
                return Content("Connected");
            }
            return Content("NotConnected");
        }
    }
}

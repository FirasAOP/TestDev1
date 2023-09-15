using Microsoft.AspNetCore.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Test;

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

        [HttpPost("testApi")]
        public IActionResult TestApi(string testParam1, string testParam2,TestModel requestBody)
        {
            if (requestBody.status)
            {
                //do something
            }
            var internalObj = new
            {
                title = "Software Developer",
                Age = 30,
                Company = "AlphaOmega"
            };
            var Obj = new
            {
                name = "firas",
                email = "Firas@gmail.com",
                address = "Ramallah",
                Job = "Developer",
                Details = "more details come later...",
                InternalDetails= internalObj
            };
            return  new JsonResult(Obj);
        }



    }
}

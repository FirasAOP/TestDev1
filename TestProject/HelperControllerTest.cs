using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Helpers;
using WebApplication1.Hub;
using Xunit;

namespace TestProject
{
    public class HelperControllerTest
    {
        private readonly HelperController _helperController;
        private readonly CheckHelper _checkHelper;
        private readonly WebSocketHub _webSocketHub;

        public HelperControllerTest()
        {   
            _webSocketHub = new WebSocketHub();
             _checkHelper = new CheckHelper(_webSocketHub);

            _helperController = new HelperController(_checkHelper);
        }

        [Fact]
        public void IsConnectedToInternet_Success() 
        {
            //Arrange

            //Act
            CheckHelper c = new CheckHelper(_webSocketHub);
            var r = c.IsConnectedToInternet();
            var result = _helperController.IsConnectedToInternet();
            var resultType = result as JsonResult;
            var resultValue = resultType?.Value;
            var jsonString = JsonConvert.SerializeObject(resultValue);
            dynamic jsonObj = JObject.Parse(jsonString);
            string status = jsonObj.Status;
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<JsonResult>(resultType);
            Assert.Equal("Connected", status);
            
        }

        [Fact]
        public void IsConnectedToInternet_Fail()
        {
            //Arrange

            //Act
            var result = _helperController.IsConnectedToInternet();
            var resultType = result as JsonResult;
            var resultValue = resultType?.Value;
            var jsonString = JsonConvert.SerializeObject(resultValue);
            dynamic jsonObj = JObject.Parse(jsonString);
            string status = jsonObj.Status;
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<JsonResult>(resultType);
            Assert.Equal("NotConnected", status);

        }

    }
}

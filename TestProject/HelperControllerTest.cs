using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Helpers;
using Xunit;

namespace TestProject
{
    public class HelperControllerTest
    {
        private readonly HelperController _helperController;
        private readonly CheckHelper _checkHelper;
        public HelperControllerTest()
        {
             _checkHelper = new CheckHelper();
            _helperController = new HelperController(_checkHelper);
        }

        [Fact]
        public void IsConnectedToInternet_Success() 
        {
            //Arrange

            //Act
            var result = _helperController.IsConnectedToInternet();
            var resultType = result as ContentResult;
            var resultValue = resultType?.Content;
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ContentResult>(resultType);
            Assert.Equal("Connected", resultValue);
            
        }

        [Fact]
        public void IsConnectedToInternet_Fail()
        {
            //Arrange

            //Act
            var result = _helperController.IsConnectedToInternet();
            var resultType = result as ContentResult;
            var resultValue = resultType?.Content;
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ContentResult>(resultType);
            Assert.Equal("NotConnected", resultValue);

        }

    }
}

using WebApplication1.Helpers;
using WebApplication1.Hub;

namespace TestProject
{
    public class CheckHelperTest
    {
        private readonly CheckHelper _checkHelper;
        private readonly WebSocketHub _webSocketHub;
        public CheckHelperTest()
        {
            _webSocketHub = new WebSocketHub();
            _checkHelper = new CheckHelper(_webSocketHub);
        }
        [Fact]
        public void IsConnectedToInternet_Success()
        {
            //Arrange

            //Act
            var result = _checkHelper.IsConnectedToInternet();
            var resultType = result;
            var resultValue = resultType;
            //Assert
            Assert.IsType<bool>(resultValue);
            Assert.True(resultValue);
        }
        [Fact]
        public void IsConnectedToInternet_Fail()
        {
            //Arrange

            //Act
            var result = _checkHelper.IsConnectedToInternet();
            var resultType = result;
            var resultValue = resultType;
            //Assert
            Assert.IsType<bool>(resultValue);
            Assert.False(resultValue);
        }
    }
}

using WebApplication1.Helpers;

namespace TestProject
{
    public class CheckHelperTest
    {
        private readonly CheckHelper _checkHelper;

        public CheckHelperTest()
        {
            _checkHelper = new CheckHelper();
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

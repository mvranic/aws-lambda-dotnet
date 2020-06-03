using Xunit;

namespace Amazon.Lambda.RuntimeSupport.IntegrationTests {
    public class PlainWapperBootstrapTest {
        [Fact]
        public void Run_PlainEchoFunction_EchoNullString() {
            // Prepare:
            var echoFunction = new EchoFunction();
            // Act:
            var result = echoFunction.CustomHandler(null);
            // Assert:
            Assert.Null(result);
        }

        [Fact]
        public void Run_PlainEchoFunction_EchoEmptyString() {
            // Prepare:
            var echoFunction = new EchoFunction();
            // Act:
            var result = echoFunction.CustomHandler("");
            // Assert:
            Assert.Equal("", result);
        }

        [Fact]
        public void Run_PlainEchoFunction_EchoAString() {
            // Prepare:
            var echoFunction = new EchoFunction();
            string value = "A string value";
            // Act:
            var result = echoFunction.CustomHandler(value);
            // Assert:
            Assert.Equal(value, result);
        }
    }
}

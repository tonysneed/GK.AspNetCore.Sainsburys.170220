using Xunit;

namespace Tests
{
    public class ClassLibraryTests
    {
        [Fact]
        public void Echo_Should_Return_Upper_Case() 
        {
            // Arrange
            var input = "hello world!";
            var expected = "HELLO WORLD!";

            // Act
            var output = ClassLibrary.Hello.Echo(input);

            // Assert
            Assert.Equal(expected, output);
        }
    }
}

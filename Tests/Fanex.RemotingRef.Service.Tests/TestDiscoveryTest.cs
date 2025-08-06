namespace Fanex.RemotingRef.Service.Tests
{
    using Xunit;

    public class TestDiscoveryTest
    {
        [Fact]
        public void SimpleTest_ShouldPass()
        {
            // Arrange
            const int expected = 2;

            // Act
            var actual = 1 + 1;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 3, 5)]
        [InlineData(0, 0, 0)]
        public void TheoryTest_ShouldPass(int a, int b, int expected)
        {
            // Act
            var actual = a + b;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
} 
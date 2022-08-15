namespace CsharpMacros.UnitTests;

public class HealthCheks
{
    [Fact]
    public void CanTest()
        => Assert.Equal(5, 2 + 3);
}
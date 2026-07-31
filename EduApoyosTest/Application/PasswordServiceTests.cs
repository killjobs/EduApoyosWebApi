using EduApoyosApplication;

namespace EduApoyosTest.Application;

public class PasswordServiceTests
{
    private readonly PasswordService _service;

    public PasswordServiceTests()
    {
        _service = new PasswordService();
    }

    [Fact]
    public void HashPassword_Should_ReturnDifferentValue_FromOriginalPassword()
    {
        const string password = "Password123";

        var hash = _service.HashPassword(password);

        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_Should_ReturnTrue_When_PasswordMatches()
    {
        const string password = "Password123";
        var hash = _service.HashPassword(password);

        var result = _service.VerifyPassword(hash, password);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_Should_ReturnFalse_When_PasswordDoesNotMatch()
    {
        var hash = _service.HashPassword("Password123");

        var result = _service.VerifyPassword(hash, "Password456");

        Assert.False(result);
    }
}
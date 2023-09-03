using RegisterUserValidatorClass = VaultDomain.Commands.RegisterUser.RegisterUserValidator;

namespace VaultDomain.Tests.Commands.RegisterUserValidator
{
    public class Should_pass
    {
        [Fact]
        public void When_UserName_and_Password_are_valid()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("email@email.com", "validp@ssw0rd"));

            Assert.True(result.IsValid);
        }
    }
}

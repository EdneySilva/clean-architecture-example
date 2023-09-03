using RegisterUserValidatorClass = VaultDomain.Commands.RegisterUser.RegisterUserValidator;

namespace VaultDomain.Tests.Commands.RegisterUserValidator
{
    public class Should_fail
    {
        [Fact]
        public void When_UserName_is_empty()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand(string.Empty, "validpdw"));
            
            Assert.True(result.Errors.Any(a => a.PropertyName == "UserName" && a.ErrorCode == "NotEmptyValidator"));
        }

        [Fact]
        public void When_UserName_is_null()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand(null, "validpdw"));

            Assert.True(result.Errors.Any(a => a.PropertyName == "UserName" && a.ErrorCode == "NotEmptyValidator"));
        }

        [Fact]
        public void When_UserName_is_not_an_email()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("itisnotanemail@", "validpdw"));

            Assert.True(result.Errors.Any(a => a.PropertyName == "UserName" && a.ErrorCode == "EmailValidator"));
        }

        [Fact]
        public void When_Password_is_empty()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("email@email.com", string.Empty));

            Assert.True(result.Errors.Any(a => a.PropertyName == "Password" && a.ErrorCode == "NotEmptyValidator"));
        }

        [Fact]
        public void When_Password_is_null()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("email@email.com", null));

            Assert.True(result.Errors.Any(a => a.PropertyName == "Password" && a.ErrorCode == "NotEmptyValidator"));
        }

        [Fact]
        public void When_Password_length_lower_than_8()
        {
            var validator = new RegisterUserValidatorClass();
            var result = validator.Validate(new VaultDomain.Commands.RegisterUser.RegisterUserCommand("email@email.com", "short"));

            Assert.True(result.Errors.Any(a => a.PropertyName == "Password" && a.ErrorCode == "MinimumLengthValidator"));
        }
    }
}

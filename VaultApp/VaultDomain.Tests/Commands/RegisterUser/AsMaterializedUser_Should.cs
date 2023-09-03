namespace VaultDomain.Tests.Commands.RegisterUser
{
    public class AsMaterializedUser_Should
    {
        [Fact]
        public void Return_an_instance_from_User_with_same_values_on_the_command()
        {
            var command = new VaultDomain.Commands.RegisterUser.RegisterUserCommand("test@email.com", "validpassword");
            var user = command.AsMaterializedUser();
            Assert.NotNull(user);
            Assert.Equal(user.UserName, command.UserName);
            Assert.Equal(user.Password, command.Password);
        }
    }
}

using SecretHasherService = VaultApplication.Services.SecretHasher;

namespace VaultApplication.Tests.Services.SecretHasher
{
    public class Verify_Should
    {
        [Fact]
        public void Return_true_when_input_matches_with_a_stored_pwd()
        {
            var storedPwd = SecretHasherService.Hash("123456");
            Assert.True(SecretHasherService.Verify("123456", storedPwd));
        }

        [Fact]
        public void Return_false_when_input_does_not_matches_with_a_stored_pwd()
        {
            var storedPwd = SecretHasherService.Hash("123456");
            Assert.False(SecretHasherService.Verify("1234567", storedPwd));
        }
    }
}

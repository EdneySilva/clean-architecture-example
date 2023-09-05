using System.Net.Http.Json;
using VaultAppApi.Controllers;
using VaultAppApi.Tests.Infrastructure;

namespace VaultAppApi.Tests
{
    public class UserControllerTests : IDisposable
    {
        private readonly VaultApp<UsersController> _application;
        private readonly HttpClient _client;

        public UserControllerTests()
        {
            _application = new VaultApp<UsersController>();
            _client = _application.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _application.Dispose();
        }

        [Fact]
        public async Task RegisterUserWithSuccess()
        {
            var response = await _client
                .PostAsJsonAsync($"/users", new
                {
                    UserName = "mail@mail.com",
                    Password = "12345678P@ssword"
                });
            var result = await response.Content.ReadFromJsonAsync<Result>();
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task AuthenticateUserSuccesfully()
        {
            await RegisterUserWithSuccess();
            var response = await _client
                .PostAsJsonAsync($"/users/authenticate", new
                {
                    UserName = "mail@mail.com",
                    Password = "12345678P@ssword"
                });
            var result = await response.Content.ReadFromJsonAsync<Result>();
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task AuthenticateUserFailed()
        {
            await RegisterUserWithSuccess();
            var response = await _client
                .PostAsJsonAsync($"/users/authenticate", new
                {
                    UserName = "mail@mail.com",
                    Password = "invalidpassword"
                });
            var result = await response.Content.ReadFromJsonAsync<Result>();
            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
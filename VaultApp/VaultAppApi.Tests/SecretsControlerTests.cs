using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using VaultAppApi.Controllers;
using VaultAppApi.Tests.Infrastructure;

namespace VaultAppApi.Tests
{
    public class SecretsControlerTests : IDisposable
    {
        private readonly VaultApp<UsersController> _application;
        private readonly HttpClient _client;

        public SecretsControlerTests()
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
        public async Task CreateSecretSuccesfully()
        {
            var userToken = await SetupUserAccount();

            _client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", userToken);
            var response = await _client
                .PostAsJsonAsync($"/secrets", new
                {
                    Owner = "mail@mail.com",
                    Name = "pwd",
                    Value = "12345678P@ssword"
                });
            var result = await response.Content.ReadFromJsonAsync<Result>();
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateDuplicateSecretFail()
        {
            var userToken = await SetupUserAccount();

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", userToken);
            await _client
                .PostAsJsonAsync($"/secrets", new
                {
                    Owner = "mail@mail.com",
                    Name = "pwd",
                    Value = "12345678P@ssword"
                });
            var response = await _client
                .PostAsJsonAsync($"/secrets", new
                {
                    Owner = "mail@mail.com",
                    Name = "pwd",
                    Value = "12345678P@ssword"
                });
            var resultString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(resultString);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.False(response.IsSuccessStatusCode);
            Assert.False(result.Success);
        }

        private async Task<string> SetupUserAccount()
        {
            var response = await _client
                .PostAsJsonAsync($"/users", new
                {
                    UserName = "mail@mail.com",
                    Password = "12345678P@ssword"
                });
            var result = await response.Content.ReadFromJsonAsync<Result>();
            var payload = ((JsonElement)result.Payload);
            return payload.GetProperty("token").ToString();
        }        
    }
}
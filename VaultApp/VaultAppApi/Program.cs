using VaultAppApi.Extensions.Security;
using VaultApplication.DependencyInjections;
using VaultDomain.DependencyInjection;
using VaultInfrastructure.DependencyInjection;

namespace VaultAppApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.ConfigureJwtAuthentication(builder.Configuration.GetSection("Token").Get<string>());
            builder.Services.AddDomainDependencies();
            builder.Services.AddVaultApplication();
            builder.Services.AddVaultStorageSql(builder.Configuration.GetConnectionString("DefaultConnectionString"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
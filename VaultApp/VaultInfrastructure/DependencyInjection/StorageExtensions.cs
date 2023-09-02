using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data;
using VaultInfrastructure.Data.Commands.User;

namespace VaultInfrastructure.DependencyInjection
{
    public static class StorageExtensions
    {
        public static IServiceCollection AddVaultStorageSql(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new DbConnectionString(connectionString));
            services.AddScoped<IRequestHandler<RegisterUserCommand, Result>, RegisterUserCommandHandler>();
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            return services;
        }
    }
}

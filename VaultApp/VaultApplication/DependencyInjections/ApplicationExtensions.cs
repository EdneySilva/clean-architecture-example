using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VaultApplication.Behaviors;
using VaultApplication.Users.Commands;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.ValueObjects;

namespace VaultApplication.DependencyInjections
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddVaultApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<RegisterUserCommand, Result>), typeof(ValidationBehaviour<RegisterUserCommand>));
                cfg.AddBehavior(typeof(IPipelineBehavior<RegisterUserCommand, Result>), typeof(RegisterUserCommandHandler));
            });
            return services;
        }
    }
}

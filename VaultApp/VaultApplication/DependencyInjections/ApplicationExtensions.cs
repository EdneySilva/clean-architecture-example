using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VaultApplication.Behaviors;
using VaultApplication.Secrets.Commands;
using VaultApplication.Users.Commands;
using VaultDomain.Commands.AuthenticateUser;
using VaultDomain.Commands.CreateSecret;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.Commands.UpdateSecret;
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
                cfg.AddBehavior(typeof(IPipelineBehavior<AuthenticateUserCommand, Result>), typeof(ValidationBehaviour<AuthenticateUserCommand>));
                cfg.AddBehavior(typeof(IPipelineBehavior<RegisterUserCommand, Result>), typeof(ValidationBehaviour<RegisterUserCommand>));
                cfg.AddBehavior(typeof(IPipelineBehavior<RegisterUserCommand, Result>), typeof(RegisterUserCommandHandler));
                cfg.AddBehavior(typeof(IPipelineBehavior<CreateSecretCommand, Result>), typeof(ValidationBehaviour<CreateSecretCommand>));
                cfg.AddBehavior(typeof(IPipelineBehavior<CreateSecretCommand, Result>), typeof(CreateSecretCommandHandler));

                cfg.AddBehavior(typeof(IPipelineBehavior<UpdateSecretCommand, Result>), typeof(ValidationBehaviour<UpdateSecretCommand>));
                cfg.AddBehavior(typeof(IPipelineBehavior<UpdateSecretCommand, Result>), typeof(UpdateSecretCommandHandler));
            });
            return services;
        }
    }
}

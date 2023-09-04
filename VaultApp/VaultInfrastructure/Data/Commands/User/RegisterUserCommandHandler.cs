using MediatR;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.ValueObjects;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.User;

namespace VaultInfrastructure.Data.Commands.User
{
    internal class RegisterUserCommandHandler : BaseStorageCommandHandler<RegisterUserCommand, Result>
    {
        private readonly IRepository<VaultDomain.Entities.User> _repository;
        public RegisterUserCommandHandler(DbConnectionString connectionString, IRepository<VaultDomain.Entities.User> repository, IMediator mediator)
            : base(connectionString, mediator)
        {
            _repository = repository;
        }

        public override async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.AsMaterializedUser();
            await DispatchAllEvents(user.TakeEvents());
            return await _repository.InsertAsync(new RegisterUserSqlQuery(user));
        }
    }
}

using MediatR;
using VaultDomain.Queries.User;
using VaultInfrastructure.Data.Abstractions;
using VaultInfrastructure.Data.SqlQueries.User;

namespace VaultInfrastructure.Data.Commands.User
{
    internal class UserQueryCommandHandler : IRequestHandler<UserByUserName, VaultDomain.Entities.User?>
    {
        private readonly IRepository<VaultDomain.Entities.User> _repository;

        public UserQueryCommandHandler(IRepository<VaultDomain.Entities.User> repository)
        {
            _repository = repository;
        }

        public async Task<VaultDomain.Entities.User?> Handle(UserByUserName request, CancellationToken cancellationToken)
        {
            return await _repository.FirstOrDefaultAsync(new UserByUserNameSqlQuery(new VaultDomain.Entities.User
            {
                UserName = request.UserName,
            }));
        }
    }
}

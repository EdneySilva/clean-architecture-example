using Dapper;
using MediatR;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.ValueObjects;

namespace VaultInfrastructure.Data.Commands.User
{
    internal class RegisterUserCommandHandler : BaseStorageCommandHandler<RegisterUserCommand, Result>
    {
        public RegisterUserCommandHandler(DbConnectionString connectionString, IMediator mediator)
            : base(connectionString, mediator)
        {
        }

        public override async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.AsMaterializedUser();
                List<string> notes = new List<string>();
                using (var connection = CreateConnection())
                {
                    var sql = "INSERT INTO [Users] VALUES (@UserName, @Password, @CreatedAt, @CurrentStatus)";

                    var anonymousCustomer = new
                    {
                        user.UserName,
                        user.Password,
                        user.CreatedAt,
                        CurrentStatus = (int)user.Status,
                    };
                    var rowsAffected = await connection.ExecuteAsync(sql, anonymousCustomer);
                    notes.Add("User created successfully");
                }
                await DispatchAllEvents(user.TakeEvents());
                var result = new Result(notes);
                return result;
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}

using MediatR;

namespace VaultDomain.Queries.User
{
    public class UserByUserName : IRequest<Entities.User>
    {
        public string UserName { get; }
        public UserByUserName(string userName) 
        {
            UserName = userName;
        }
    }
}

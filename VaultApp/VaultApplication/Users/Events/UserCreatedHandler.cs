using MediatR;
using VaultDomain.Events;

namespace VaultApplication.Users.Events
{
    public class UserCreatedHandler : INotificationHandler<UserCreated>
    {
        public Task Handle(UserCreated notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

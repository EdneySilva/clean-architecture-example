using VaultDomain.Entities;

namespace VaultDomain.Events
{
    public class UserCreated : BaseEvent
    {
        private readonly User _user;

        public UserCreated(User user)
        {
            _user = user;
        }

        public User User { get => _user; }
    }
}

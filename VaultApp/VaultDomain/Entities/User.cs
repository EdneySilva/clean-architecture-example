using System.Diagnostics;
using VaultDomain.Enums;
using VaultDomain.Events;

namespace VaultDomain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            AddEvent(new UserCreated(this));
        }

        public string UserName { get; set; }

        [DebuggerHidden]
        public string Password { get; set; }

        public UserStatus Status { get; set; }
    }
}

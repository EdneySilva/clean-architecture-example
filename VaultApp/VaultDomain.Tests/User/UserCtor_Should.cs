namespace VaultDomain.Tests.User
{
    public class UserCtor_Should
    {
        [Fact]
        public void Produce_event_UserCreated()
        {
            var user = new Entities.User();
            var events = user.TakeEvents();
            Assert.Equal(events.First().GetType(), typeof(Events.UserCreated));
        }
    }
}

using EventSourcing_Example_With_EventStore.EventTypes;
using EventSourcing_Example_With_EventStore.Models;
using EventStore.ClientAPI;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Aggregates.Repositories
{
  public class UserStreamRepository : StreamRepository
  {
    public UserStreamRepository(IEventStoreConnection connection) : base(connection)
    {
    }
    
    public async Task<User> GetUserProduction(string streamName)
    {
      dynamic events = await GetEvents(streamName);
      User user = new();
      foreach (var @event in events)
      {
        switch (@event.Data)
        {
          case UserCreated o:
            user.Id = o.UserId;
            user.Name = o.Name;
            user.UserName = o.UserName;
            user.Email = o.Email;
            user.EmailApprove = o.EmailApprove;
            break;
          case UserNameChanged o:
            user.UserName = o.NewUserName;
            break;
          case UserEmailApproved o:
            user.EmailApprove = true;
            break;
        }
      }
      return user;
    }
  }
}

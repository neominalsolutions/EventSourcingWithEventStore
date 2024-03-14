using EventSourcing_Example_With_EventStore.Aggregates.Abstractions;
using EventSourcing_Example_With_EventStore.EventTypes;
using EventSourcing_Example_With_EventStore.Exceptions;
using EventSourcing_Example_With_EventStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Aggregates
{
  // User Sınıfı ile ilgili eventleri yönettiğimiz sınıf.
  public class UserStreamAggregate : StreamAggregate
  {
    //Kullanıcı oluşturulduğunda
    public void Created(User model)
    {
      SetStreamName($"user-{model.Id}");

      if (CheckStreamName())
        throw new StreamNotFoundException();

      UserCreated userCreated = new()
      {
        UserId = model.Id,
        Email = model.Email,
        EmailApprove = model.EmailApprove,
        Name = model.Name,
        UserName = model.UserName
      };
      events.Add(userCreated);
    }
    //Kullanıcı adı değiştirildiğinde
    public void UserNameChanged(string newUserName, string userId)
    {
      SetStreamName($"user-{userId}");

      if (CheckStreamName())
        throw new StreamNotFoundException();

      UserNameChanged userNameChanged = new()
      {
        NewUserName = newUserName,
        UserId = userId
      };
      events.Add(userNameChanged);
    }
    //Kullanıcı email onaylandığında
    public void EmailApproved(string userId)
    {
      SetStreamName($"user-{userId}");

      if (CheckStreamName())
        throw new StreamNotFoundException();

      UserEmailApproved userEmailApproved = new()
      {
        UserId = userId
      };
      events.Add(userEmailApproved);
    }




  }
}

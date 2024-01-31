using EventSourcing_Example_With_EventStore.EventTypes;
using EventSourcing_Example_With_EventStore.Models;
using EventStore.ClientAPI;
using MongoDB.Driver;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Subscriptions
{
  public static class StreamSubscriber
  {
    // connection'a extension olarak subscriber tanımladık
    public static async Task SubscribeToUserStreams(this IEventStoreConnection connection, IMongoCollection<User> userCollection)
    {
      await connection.SubscribeToAllAsync(
          resolveLinkTos: true,
          eventAppeared: async (eventStoreSubscription, resolvedEvent) =>
          {
            //Return Type class'ı farklı bir class library ya da proje de ise bu şekilde metinsel olarak nitelendirilip
            //Type.GetType(...)'a verilmelidir.
            var returnType = $"{Encoding.UTF8.GetString(resolvedEvent.Event.Metadata)}";
            Type eventType = Type.GetType(returnType);
            object @event = JsonSerializer.Deserialize(Encoding.UTF8.GetString(resolvedEvent.Event.Data), eventType);

            User user = null;
            UpdateDefinition<User> update = null;
            switch (@event)
            {
              case UserCreated e:
                //User'ı veritabanına ekle
                user = new()
                {
                  Id = e.UserId,
                  Email = e.Email,
                  EmailApprove = e.EmailApprove,
                  Name = e.Name,
                  UserName = e.UserName
                };
                await userCollection.InsertOneAsync(user);
                break;
              case UserNameChanged e:
                //User'ı veritabanından çek ve adını güncelle
                update = Builders<User>.Update.Set(u => u.UserName, e.NewUserName);
                await userCollection.UpdateOneAsync(f => f.Id == e.UserId, update);
                break;
              case UserEmailApproved e:
                //User'ı veritabanından çek ve email'i onayla
                update = Builders<User>.Update.Set(u => u.EmailApprove, true);
                await userCollection.UpdateOneAsync(f => f.Id == e.UserId, update);
                break;
            }

          },
          subscriptionDropped: (eventStoreSubscription, subscriptionDropReason, exception) => Console.WriteLine($"Bağlantı kopmuştur. {subscriptionDropReason}")
          );
    }
  }
}

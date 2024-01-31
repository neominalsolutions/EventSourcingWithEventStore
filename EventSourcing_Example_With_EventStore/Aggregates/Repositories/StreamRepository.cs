using EventSourcing_Example_With_EventStore.Aggregates.Abstractions;
using EventSourcing_Example_With_EventStore.EventTypes;
using EventSourcing_Example_With_EventStore.Models;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Aggregates.Repositories
{
  public abstract class StreamRepository
  {
    readonly IEventStoreConnection _connection;
    public StreamRepository(IEventStoreConnection connection)
    {
      _connection = connection;

    }
    //Oluşturulan event'leri Event Store'a kaydeder.
    public async Task SaveAsync<T>(T aggregate) where T : Aggregate, new()
    {
      var options = new JsonSerializerOptions
      {
        WriteIndented = true
      };

      List<EventData> events = aggregate.GetEvents
          .Select(@event => new EventData(
              eventId: Guid.NewGuid(),
              type: @event.GetType().Name,//type : Event Store'a kaydedilecek olan event'in türünü sınıf olarak bildiriyoruz.
              isJson: true,
              data: Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(
                  value: @event,
                  inputType: @event.GetType(),
                  options
              )),
              metadata: Encoding.UTF8.GetBytes(@event.GetType().FullName))//metadata : Metadata olarak binary formatta ilgili event'in FullName bilgisini yani namespace ile birlikte full class adını tutmaktayız. Bu bilgiyi, event'leri 'Read Data Store'da güncelleme yaparken hangi event'in gerçekleştiğini ayırt edebilmek için kullanacağız. 
         ).ToList();

      if (!events.Any())
        return;

      //Event'ler gönderiliyor...
      await _connection.AppendToStreamAsync(aggregate.StreamName, ExpectedVersion.Any, events);
      aggregate.GetEvents.Clear();
    }
    //Event Store'dan belirtilen Stream'de ki event'leri getirir.
    public async Task<dynamic> GetEvents(string streamName)
    {
      long nextSliceStart = 0L;
      List<ResolvedEvent> events = new();
      StreamEventsSlice readEvents = null; // EventStore'dan tek bir event okumayı temsil eden sınıf, bir sonraki eventi okumak için NextEventNumber kullanırız.
      do
      {
        // ReadStreamEventsBackwardAsync 
        // eventleri ileri ve geri yönlü okuyabiliriz.
        // await connection.ReadEventAsync("StreamName", 3, true);
        // sequencial olarak event okuması yapmak için ReadEventAsync kullanırız.
        // DeleteResult deleteResult = await connection.DeleteStreamAsync("Example-Event-3", ExpectedVersion.Any); oluşturulan bir stream'i silebiliriz.
        readEvents = await _connection.ReadStreamEventsForwardAsync(
            stream: streamName,
            start: nextSliceStart,
            count: 4096,
            resolveLinkTos: true
            );

        if (readEvents.Events.Length > 0)
          events.AddRange(readEvents.Events);

        nextSliceStart = readEvents.NextEventNumber;
      } while (!readEvents.IsEndOfStream); // stream sonuna gelene kadar event okuma işlemi


      return events.Select(@event => new
      {
        @event.Event.EventNumber, // Sequencial
        @event.Event.EventType, // Created,Updated vs
        @event.Event.Created,  // Event Create edildiği tarih.
        @event.Event.EventId, // EventId Unique tanımlıdır
        @event.Event.EventStreamId,
        Data = JsonSerializer.Deserialize(
              json: Encoding.UTF8.GetString(@event.Event.Data),
              returnType: Type.GetType(Encoding.UTF8.GetString(@event.Event.Metadata)) //returnType : Yukarıda 'SaveAsync' metodunda metadata olarak tutulan event class'ının tam adı, burada ilgili event'in özgün sınıfına dönüştürülürken kullanılmaktadır.
              ),
        Metadata = Encoding.UTF8.GetString(@event.Event.Metadata) // Eventin bulunduğu classPath
      });
    }
    
  }
}

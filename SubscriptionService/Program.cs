
using EventStore.ClientAPI;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {

      IEventStoreConnection connection = EventStoreConnection.Create(
                connectionString: "ConnectTo=tcp://localhost:1115;DefaultUserCredentials=admin:changeit;UseSslConnection=true;TargetHost=eventstore.org;ValidateServer=false",
                connectionName: "API_Application",
                builder: ConnectionSettings.Create().KeepReconnecting()
            );

      connection.SubscribeToAllFrom(
    lastCheckpoint: Position.Start,
    settings: CatchUpSubscriptionSettings.Default,
    eventAppeared: (sub, evt)
        => Console.Out.WriteLineAsync("Event appeared"),
    liveProcessingStarted: sub
        => Console.WriteLine("Processing live"),
    subscriptionDropped: (sub, reason, exception)
        => Console.WriteLine($"Subscription dropped: {exception.Message}")
  );

    })
    .Build();

await host.RunAsync();


using EventSourcing_Example_With_EventStore.Aggregates;
using EventSourcing_Example_With_EventStore.Aggregates.Repositories;
using EventSourcing_Example_With_EventStore.Database;
using EventSourcing_Example_With_EventStore.Models;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(opt =>
{
  opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

IEventStoreConnection connection = EventStoreConnection.Create(
                connectionString: "ConnectTo=tcp://localhost:1115;DefaultUserCredentials=admin:changeit;UseSslConnection=true;TargetHost=eventstore.org;ValidateServer=false",
                connectionName: "API_Application",
                builder: ConnectionSettings.Create().KeepReconnecting()
            );

connection.ConnectAsync().GetAwaiter().GetResult();
builder.Services.AddSingleton(connection);
// services.AddSingleton<StreamRepository>();
builder.Services.AddSingleton<UserStreamRepository>();
builder.Services.AddSingleton<UserStreamAggregate>();

//IMongoCollection<User> userCollection = MongoDBConfiguration.Collection<User>("users");
//// tüm stream subscribe olduk sadece user ile ilgi eventleri dinleyip, mongodbde production oluþturduk.
// await connection.SubscribeToUserStreams(userCollection);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




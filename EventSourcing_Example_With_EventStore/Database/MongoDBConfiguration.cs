

using MongoDB.Driver;

namespace EventSourcing_Example_With_EventStore.Database
{
  public static class MongoDBConfiguration
  {
    static IMongoDatabase database;
    public static void GetDatabase()
    {
      if (database == null)
      {
        MongoClient mongoClient = new("mongodb://localhost:27017");
        database = mongoClient.GetDatabase("EventStoreDB");
      }
    }
    public static IMongoCollection<T> Collection<T>(string collectionName)
    {
      // collection çağırıken mongodb bağlantı kurduk.
      GetDatabase();
      IMongoCollection<T> collection = database.GetCollection<T>(collectionName);
      return collection;
    }
  }
}

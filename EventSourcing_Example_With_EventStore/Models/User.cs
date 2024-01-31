using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing_Example_With_EventStore.Models
{
  public class User
  {

    [BsonElement("_id")]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("username")]
    public string UserName { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("email_approve")]
    public bool EmailApprove { get; set; }

  }
}

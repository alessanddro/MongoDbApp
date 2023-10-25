using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDbApp.Api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
    }
}

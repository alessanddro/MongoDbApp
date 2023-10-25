using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbApp.Api.Models;

public class Cover
{
    public bool flag { get; set; } = false;
    public string cover { get; set; }
}

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string BookName { get; set; } = null!;

    public decimal Price { get; set; }

    public string Category { get; set; } = null!;

    public string Author { get; set; } = null!;

    public List<Cover>? BookCover { get; set; } = new List<Cover>();
}
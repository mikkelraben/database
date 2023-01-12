using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace database.Models
{
    public class Todo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Data { get; set; } = string.Empty;
        public bool Done { get; set; } = false;
    }
}

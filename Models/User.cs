using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationsApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string username { get; set; }
        
        public string passwordHash { get; set; }
    }
}

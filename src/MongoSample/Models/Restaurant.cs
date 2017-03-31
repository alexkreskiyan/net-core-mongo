using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoSample.Models
{
    public class Restaurant
    {
        public ObjectId Id { get; set; }

        [BsonElement("restaurant_id")]
        public string RestaurantId { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public string Borough { get; set; }

        public string Cuisine { get; set; }

        public GradeEntry[] Grades { get; set; }
    }
}
using MongoDB.Bson.Serialization.Attributes;

namespace MongoSample.Models
{
    public class Address
    {
        public string Building { get; set; }

        [BsonElement("coord")]
        public double[] Coordinates { get; set; }

        public string Street { get; set; }

        public string Zipcode { get; set; }
    }
}
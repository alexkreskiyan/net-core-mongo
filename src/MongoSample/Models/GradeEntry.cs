using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoSample.Models
{
    public class GradeEntry
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }

        public string Grade { get; set; }

        public int Score { get; set; }
    }
}
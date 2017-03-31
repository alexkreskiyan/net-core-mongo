using MongoDB.Driver;
using MongoSample.Models;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MongoSample.Commands
{
    public class FindCommand : ICommand
    {
        private readonly IMongoCollection<Restaurant> collection;

        public FindCommand(IMongoCollection<Restaurant> collection)
        {
            this.collection = collection;
        }

        public string Name { get; } = "find";

        public async Task Run(string[] args)
        {
            var restaurants = await collection
                .Find(restaurant => restaurant.Address.Building == "1007")
                .Limit(1)
                .ToListAsync();

            foreach (var restaurant in restaurants)
            {
                Console.WriteLine(JsonConvert.SerializeObject(
                    restaurant,
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                ));
            }
        }
    }
}
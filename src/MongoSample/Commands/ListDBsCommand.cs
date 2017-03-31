using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoSample.Commands
{
    public class ListDBsCommand : ICommand
    {
        public string Name { get; } = "dbs";

        private readonly IMongoClient mongoClient;

        public ListDBsCommand(
            IMongoClient mongoClient
        )
        {
            this.mongoClient = mongoClient;
        }

        public async Task Run(string[] args)
        {
            var docs = await mongoClient.ListDatabases().ToListAsync();

            foreach (var doc in docs)
                Console.WriteLine(doc.GetElement("name").Value);
        }
    }
}
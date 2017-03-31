using Microsoft.Extensions.DependencyInjection;
using System;
using MongoDB.Driver;
using MongoSample.Commands;

namespace MongoSample
{
    public static class Configurator
    {
        public static IServiceProvider Configure()
        {
            var collection = new ServiceCollection();

            var mongoClient = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress("localhost", 27017)
            });
            collection.AddSingleton<IMongoClient>(mongoClient);
            collection.AddSingleton<Commander>();
            collection.AddSingleton<ListDBsCommand>();

            return collection.BuildServiceProvider();
        }

        public static Commander ConfigureCommander(Commander commander)
        {
            commander.RegisterCommand<ListDBsCommand>();

            return commander;
        }
    }
}
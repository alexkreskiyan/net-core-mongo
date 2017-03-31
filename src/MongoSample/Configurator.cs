using Microsoft.Extensions.DependencyInjection;
using System;
using MongoDB.Driver;
using MongoSample.Commands;
using MongoSample.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver.Core.Events;
using MongoDB.Bson;
using System.Linq;

namespace MongoSample
{
    public static class Configurator
    {
        public static IServiceProvider Configure()
        {
            var services = new ServiceCollection();

            RegisterConventions();

            var unloggedCommands = new[] { "isMaster", "buildInfo", "getLastError" };
            var mongoClient = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress("localhost", 27017),
                ClusterConfigurator = configurator =>
                {
                    configurator.Subscribe<CommandStartedEvent>(e =>
                    {
                        if (!unloggedCommands.Contains(e.CommandName))
                            Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                    });
                }
            });
            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton<IMongoCollection<Restaurant>>(
                mongoClient.GetDatabase("test").GetCollection<Restaurant>("restaurants")
            );
            services.AddSingleton<Commander>();
            services.AddSingleton<ListDBsCommand>();
            services.AddSingleton<FindCommand>();

            return services.BuildServiceProvider();
        }

        public static Commander ConfigureCommander(Commander commander)
        {
            commander.RegisterCommand<ListDBsCommand>();
            commander.RegisterCommand<FindCommand>();

            return commander;
        }

        private static void RegisterConventions()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camelCase", pack, type => true);
        }
    }
}
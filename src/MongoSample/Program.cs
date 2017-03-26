using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoSample
{
    public class Program
    {
        private static IServiceProvider provider = Configure();

        public static void Main()
        {
            Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            var commander = provider.GetRequiredService<Commander>();
            while (true)
                await commander.Run(Console.ReadLine()).ConfigureAwait(false);
        }

        private static IServiceProvider Configure()
        {
            var collection = new ServiceCollection();

            collection.AddSingleton<IMongoClient, MongoClient>();
            collection.AddSingleton<Commander>(GetCommander());

            return collection.BuildServiceProvider();
        }

        private static Commander GetCommander()
        {
            var commander = new Commander();

            return commander;
        }

        private class Commander
        {
            private readonly IDictionary<string, Func<string[], Task>> commands
                = new Dictionary<string, Func<string[], Task>>();

            public async Task Run(string input)
            {
                Console.WriteLine($"Running `{input}`");
                await Task.Delay(500);
                Console.WriteLine($"Done");
            }

            public void RegisterCommand(string command, Func<string[], Task> action)
            {
                commands.Add(command, action);
            }
        }
    }
}
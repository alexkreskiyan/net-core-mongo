using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

            commander.RegisterCommand("dbs", ListDBs);

            return commander;
        }

        private static async Task ListDBs(string[] args)
        {
            var client = provider.GetRequiredService<IMongoClient>();
            var docs = await (await client.ListDatabasesAsync()).ToListAsync();

            foreach (var doc in docs)
            {
                Console.WriteLine(doc.GetElement("name").Value);
            }
        }

        private class Commander
        {
            private readonly IDictionary<string, Func<string[], Task>> commands
                = new Dictionary<string, Func<string[], Task>>();

            public async Task Run(string input)
            {
                var command = commands
                    .OrderByDescending(candidate => candidate.Key.Length)
                    .FirstOrDefault(candidate => input.Contains(candidate.Key));

                if (command.Value == null)
                {
                    Console.WriteLine($"Request `{input}` doesn't match any command");
                    return;
                }

                var args = input
                    .Substring(command.Key.Length)
                    .Split(' ')
                    .Select(argument => argument.Trim())
                    .ToArray();

                await command.Value(args);
            }

            public void RegisterCommand(string command, Func<string[], Task> action)
            {
                commands.Add(command, action);
            }
        }
    }
}
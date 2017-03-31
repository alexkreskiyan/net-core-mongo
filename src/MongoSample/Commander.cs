using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoSample.Commands;

namespace MongoSample
{
    public class Commander
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IList<ICommand> commands = new List<ICommand>();

        public Commander(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void RegisterCommand<TCommand>()
            where TCommand : class, ICommand
        {
            commands.Add(serviceProvider.GetRequiredService<TCommand>());
        }

        public async Task Run(string input)
        {
            var command = commands
                .OrderByDescending(candidate => candidate.Name.Length)
                .FirstOrDefault(candidate => input.Contains(candidate.Name));

            if (command == null)
            {
                Console.WriteLine($"Request `{input}` doesn't match any command");
                return;
            }

            var args = input
                .Substring(command.Name.Length)
                .Split(' ')
                .Select(argument => argument.Trim())
                .Where(argument => argument != string.Empty)
                .ToArray();

            await command.Run(args);
        }
    }
}
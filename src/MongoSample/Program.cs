using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MongoSample
{
    public class Program
    {
        private static IServiceProvider provider = Configurator.Configure();

        public static void Main()
        {
            Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            var commander = Configurator.ConfigureCommander(provider.GetRequiredService<Commander>());
            while (true)
                await commander.Run(Console.ReadLine()).ConfigureAwait(false);
        }
    }
}
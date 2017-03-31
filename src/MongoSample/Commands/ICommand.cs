using System.Threading.Tasks;

namespace MongoSample.Commands
{
    public interface ICommand
    {
        string Name { get; }

        Task Run(string[] args);
    }
}
using System.Threading.Tasks;
using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Client
{
    public interface IClient
    {
        bool Authenticated { get; }
        Task<Result<Authentication>> Login(string username, string password);
        Task<Result<Page<Account>>> Accounts();
        Task<Result<Instrument>> Instrument(string instrumentNumber);
        Task<Result<Order>> Orders(NewOrder newOrder);
        Task<Result<Page<Position>>> Positions(string accountNumber);
        Task<Result<Quote>> Quote(string symbol);
    }
}
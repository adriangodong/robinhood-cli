using System.Threading.Tasks;
using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Client
{
    public class DeadlockWrapperClient : IClient
    {
        private readonly Deadlock.Robinhood.RobinhoodClient client;

        public DeadlockWrapperClient(string token = null)
        {
            client = new Deadlock.Robinhood.RobinhoodClient(token);
        }

        public bool Authenticated => client.Authenticated;

        public async Task<Result<Authentication>> Login(string username, string password) => await client.Login(username, password);

        public async Task<Result<Page<Account>>> Accounts() => await client.Accounts();
        public async Task<Result<Instrument>> Instrument(string instrumentNumber) => await client.Instrument(instrumentNumber);
        public async Task<Result<Order>> Orders(NewOrder newOrder) => await client.Orders(newOrder);
        public async Task<Result<Page<Position>>> Positions(string accountNumber) => await client.Positions(accountNumber);
        public async Task<Result<Quote>> Quote(string symbol) => await client.Quote(symbol);
    }
}
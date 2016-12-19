using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class LoginCommand : ICommand
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public async Task<ExecutionResult> Execute(ExecutionContext context)
        {
            using (RobinhoodClient client = new RobinhoodClient())
            {
                var result = await client.Login(Username, Password);
                if (!result.IsSuccessStatusCode)
                {
                    return new ExecutionResult()
                    {
                        LastError = $"Login failed: {result.Content}"
                    };
                }

                return new AuthenticationExecutionResult()
                {
                    AuthenticationToken = result.Data.Token
                };
            }
        }

    }
}
using System.Threading.Tasks;
using RobinhoodCli.Client;

namespace RobinhoodCli.Commands
{
    public class LoginCommand : ICommand
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public async Task<ExecutionResult> Execute(
            IClient client,
            ExecutionContext context)
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
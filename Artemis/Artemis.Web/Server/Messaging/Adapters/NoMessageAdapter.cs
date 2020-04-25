using System.Threading.Tasks;

namespace Artemis.Web.Server.Messaging.Adapters
{
    public class NoMessageAdapter : MessagingClientAdapter
    {
        public override Task<string> SendMessage(string to, string message) 
            => Task.FromResult(string.Empty);
    }
}
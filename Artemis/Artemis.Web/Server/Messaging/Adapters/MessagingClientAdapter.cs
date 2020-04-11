using System.Threading.Tasks;

namespace Artemis.Web.Server.Messaging.Adapters
{
    public abstract class MessagingClientAdapter
    {
        public abstract Task<string> SendMessage(string to, string message);
    }
}
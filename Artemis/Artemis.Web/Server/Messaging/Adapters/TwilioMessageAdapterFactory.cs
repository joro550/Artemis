using Artemis.Web.Server.Config;
using Microsoft.Extensions.Options;

namespace Artemis.Web.Server.Messaging.Adapters
{
    public class TwilioMessageAdapterFactory
    {
        private readonly TwilioConfig _config;

        public TwilioMessageAdapterFactory(IOptions<TwilioConfig> config)
            => _config = config.Value;

        public MessagingClientAdapter GetMessagingClient()
        {
            return string.IsNullOrWhiteSpace(_config.AccountSid) || string.IsNullOrWhiteSpace(_config.Token)
                ? (MessagingClientAdapter)new NoMessageAdapter()
                : new TwilioMessageAdapter(_config);
        }
    }
}
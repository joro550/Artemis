using Artemis.Web.Server.Config;
using Microsoft.Extensions.Options;

namespace Artemis.Web.Server.Messaging.Adapters
{
    public class TwilioMessageAdapterFactory
    {
        private readonly IOptions<TwilioConfig> _config;

        public TwilioMessageAdapterFactory(IOptions<TwilioConfig> config) 
            => _config = config;

        public MessagingClientAdapter GetMessagingClient()
        {
            var twilioConfig = _config.Value;
            return string.IsNullOrWhiteSpace(twilioConfig.AccountSid) || string.IsNullOrWhiteSpace(twilioConfig.Token)
                ? (MessagingClientAdapter) new NoMessageAdapter()
                : new TwilioMessageAdapter(_config);
        }
    }
}
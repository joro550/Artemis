using System;
using Twilio;
using System.Threading.Tasks;
using Artemis.Web.Server.Config;
using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;

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

    public class NoMessageAdapter : MessagingClientAdapter
    {
        public override Task<string> SendMessage(string to, string message) 
            => Task.FromResult(string.Empty);
    }




    public class TwilioMessageAdapter : MessagingClientAdapter
    {
        private readonly TwilioConfig _config;

        public TwilioMessageAdapter(IOptions<TwilioConfig> config)
        {
            _config = config.Value;
            TwilioClient.Init(config.Value.AccountSid, config.Value.Token);
        }

        public override async Task<string> SendMessage(string to, string message)
        {
            try
            {
                var messageResource =
                    await MessageResource.CreateAsync(to: to, body: message, from: _config.FromNumber);
                return messageResource.Sid;
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
    }
}
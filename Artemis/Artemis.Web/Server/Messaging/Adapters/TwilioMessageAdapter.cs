using Twilio;
using System.Threading.Tasks;
using Artemis.Web.Server.Config;
using Microsoft.Extensions.Options;
using Twilio.Rest.Api.V2010.Account;

namespace Artemis.Web.Server.Messaging.Adapters
{
    public class TwilioMessageAdapter : MessagingClientAdapter
    {
        private readonly TwilioConfig _config;

        public TwilioMessageAdapter(IOptions<TwilioConfig> config)
        {
            _config = config.Value;
            TwilioClient.Init(config.Value.AccountSid, config.Value.Token);
        }

        public override async Task<string> SendMessage(string to, string message) 
            => (await MessageResource.CreateAsync(to: to, body: message, @from: _config.FromNumber)).Sid;
    }
}
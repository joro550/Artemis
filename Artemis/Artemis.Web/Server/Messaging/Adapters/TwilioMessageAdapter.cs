using System;
using Twilio;
using System.Threading.Tasks;
using Artemis.Web.Server.Config;
using Twilio.Rest.Api.V2010.Account;

namespace Artemis.Web.Server.Messaging.Adapters
{
    public class TwilioMessageAdapter : MessagingClientAdapter
    {
        private readonly TwilioConfig _config;

        public TwilioMessageAdapter(TwilioConfig config)
        {
            _config = config;
            TwilioClient.Init(_config.AccountSid, _config.Token);
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
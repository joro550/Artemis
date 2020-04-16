using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Artemis.Web.Server.Config;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Artemis.Web.Server.Messaging
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridConfiguration _options;

        public EmailSender(IOptions<SendGridConfiguration> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message) 
            => Execute(_options.Key, subject, message, email);

        private Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_options.FromAddress, _options.User),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }
    }
}

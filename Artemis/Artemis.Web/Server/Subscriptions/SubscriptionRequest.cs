using MediatR;

namespace Artemis.Web.Server.Subscriptions
{
    public abstract class SubscriptionRequest : INotification
    {
        public string UserId { get; set; }
    }
}
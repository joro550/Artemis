namespace Artemis.Web.Server.Subscriptions
{
    public class CreateLocationSubscription : SubscriptionRequest
    {
        public int OrganizationId { get; set; }
    }

    public class CreateOrganizationSubscription : SubscriptionRequest
    {
        public int OrganizationId { get; set; }
    }

    public class CreateEventSubscription : SubscriptionRequest
    {
        public int EventId { get; set; }
    }
}
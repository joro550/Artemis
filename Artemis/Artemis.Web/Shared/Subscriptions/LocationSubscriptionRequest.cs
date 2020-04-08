namespace Artemis.Web.Shared.Subscriptions
{
    public class LocationSubscriptionRequest
    {
        public int OrganizationId { get; set; }
    }

    public class OrganizationSubscriptionRequest
    {
        public int OrganizationId { get; set; }
    }

    public class EventSubscriptionRequest
    {
        public int EventId { get; set; }
    }
}
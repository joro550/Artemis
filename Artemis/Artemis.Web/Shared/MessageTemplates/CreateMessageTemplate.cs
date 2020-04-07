namespace Artemis.Web.Shared.MessageTemplates
{
    public class CreateMessageTemplate
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
        public MessageEvent MessageEvent { get; set; }
    }
}
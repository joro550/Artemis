using System.Runtime.CompilerServices;

namespace Artemis.Web.Shared.Organizations
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }
        public string Description { get; set; }
    }
}
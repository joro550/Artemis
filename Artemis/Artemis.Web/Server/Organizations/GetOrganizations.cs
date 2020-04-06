using System.Collections.Generic;
using Artemis.Web.Shared.Organizations;
using MediatR;

namespace Artemis.Web.Server.Organizations
{
    public class GetOrganizations : IRequest<List<Organization>>
    {
        public int Count { get; set; }
        public int Offset { get; set; }
    }
}
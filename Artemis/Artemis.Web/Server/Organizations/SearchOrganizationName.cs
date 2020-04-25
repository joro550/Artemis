using MediatR;
using System.Collections.Generic;
using Artemis.Web.Shared.Organizations;

namespace Artemis.Web.Server.Organizations
{
    public class SearchOrganizationName : IRequest<List<Organization>>
    {
        public string SearchValue { get; set; }
            = string.Empty;

        public string? UserId { get; set; }

        public int Offset { get; set; }
        public int Count { get; set; }
    }
}
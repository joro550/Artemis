using Xunit;
using MediatR;
using NSubstitute;
using System.Threading.Tasks;
using Artemis.Web.Server.Users;
using Artemis.Web.Shared.Organizations;
using Artemis.Web.Server.Organizations;
using Artemis.Web.Server.Organizations.Controllers;

namespace Artemis.Web.Server.Tests.Organizations
{
    public class GetIndividualOrganization
    {
        private readonly IMediator _mediator;
        private readonly IUserAdapter _userAdapter;
        private readonly OrganizationController _controller;

        protected GetIndividualOrganization()
        {
            _userAdapter = new FakeUserAdapter();
            _mediator = Substitute.For<IMediator>();
            _controller = new OrganizationController(_mediator, _userAdapter);
        }

        public class OrganizationIsReturned : GetIndividualOrganization
        {
            private readonly Organization _organization;

            public OrganizationIsReturned()
            {
                _organization = new Organization();

                _mediator.Send(Arg.Any<GetOrganizationById>())
                    .Returns(Task.FromResult(_organization));
            }

            [Fact]
            public async Task WhenOrganizationExists()
            {
                var organization = await _controller.GetOrganization(1);
                Assert.Equal(_organization, organization);
            }
        }
    }
}

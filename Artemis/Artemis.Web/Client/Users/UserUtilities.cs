using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Artemis.Web.Shared.Employee;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Artemis.Web.Client.Users
{
    public class UserUtilities
    {
        private readonly NavigationManager _navigation;
        private readonly IAccessTokenProvider _authenticationService;

        public UserUtilities(IAccessTokenProvider authenticationService, NavigationManager navigation)
        {
            _authenticationService = authenticationService;
            _navigation = navigation;
        }

        public async Task<bool> IsEmployeeOfOrganization(int organizationId)
        {
            var tokenResult = await _authenticationService.RequestAccessToken();

            if (!tokenResult.TryGetToken(out var token)) return false;

            var httpClient = new HttpClient { BaseAddress = new Uri(_navigation.BaseUri) };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");

            var statuses = await httpClient.GetJsonAsync<EmployeeStatusResponse[]>("api/employee");
            return statuses.Any(response => response.OrganizationId == organizationId);
        }
    }
}

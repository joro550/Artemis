using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Artemis.Web.Client
{
    public class HttpClientAdapter
    {
        private readonly NavigationManager _navigationManager;
        private readonly IAccessTokenProvider _authenticationService;

        public HttpClientAdapter(IAccessTokenProvider authenticationService, NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _authenticationService = authenticationService;
        }

        public async Task<TokenResponse> CanGetToken()
        {
            var tokenResult = await _authenticationService.RequestAccessToken();
            var tokenAcquired = tokenResult.TryGetToken(out _);
            return new TokenResponse(tokenAcquired, tokenResult);
        }

        public async Task<T> GetJsonAsync<T>(string requestUri)
        {
            var httpClient = await SetUpClient();
            return await httpClient.GetJsonAsync<T>(requestUri);
        }

        public async Task<T> PostJsonAsync<T>(string requestUri, object content)
        {
            var httpClient = await SetUpClient();
            return await httpClient.PostJsonAsync<T>(requestUri, content);
        }

        public async Task PostJsonAsync(string requestUri, object content)
        {
            var httpClient = await SetUpClient();
            await httpClient.PostJsonAsync(requestUri, content);
        }

        public async Task PutJsonAsync(string requestUri, object content)
        {
            var httpClient = await SetUpClient();
            await httpClient.PutJsonAsync(requestUri, content);
        }

        private async Task<HttpClient> SetUpClient()
        {
            var tokenResult = await _authenticationService.RequestAccessToken();
            var tokenAcquired = tokenResult.TryGetToken(out var token);

            var httpClient = new HttpClient {BaseAddress = new Uri(_navigationManager.BaseUri)};
            if (tokenAcquired)
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");
            return httpClient;
        }
    }

    public class TokenResponse
    {
        public TokenResponse(bool success, AccessTokenResult token)
        {
            Token = token;
            Success = success;
        }

        public bool Success { get; }
        public AccessTokenResult Token { get; }
    }
}
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Artemis.Web.Client
{
    public class HttpClientAdapter
    {
        private readonly NavigationManager _navigationManager;
        private readonly IAccessTokenProvider _authenticationService;

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };

        public HttpClientAdapter(IAccessTokenProvider authenticationService, NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _authenticationService = authenticationService;
        }

        public async Task<bool> CanGetToken()
        {
            var tokenResult = await _authenticationService.RequestAccessToken();
            return tokenResult.TryGetToken(out _);
        }

        public async Task<TokenResponse> CanGetToken2()
        {
            var tokenResult = await _authenticationService.RequestAccessToken();
            var tokenAcquired = tokenResult.TryGetToken(out var token);
            return new TokenResponse(tokenAcquired, tokenResult);
        }

        public async Task<T> GetJsonAsync<T>(string requestUri)
        {
            var tokenResult = await _authenticationService.RequestAccessToken();
            var tokenAcquired = tokenResult.TryGetToken(out var token);

            var httpClient = new HttpClient { BaseAddress = new Uri(_navigationManager.BaseUri) };
            if(tokenAcquired)
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");

            return await httpClient.GetJsonAsync<T>(requestUri);
        }

        public async Task<T> PostJsonAsync<T>(string requestUri, object content)
        {
            var tokenResult = await _authenticationService.RequestAccessToken();
            var tokenAcquired = tokenResult.TryGetToken(out var token);

            var httpClient = new HttpClient { BaseAddress = new Uri(_navigationManager.BaseUri) };
            if (tokenAcquired)
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Value}");

            return await httpClient.PostJsonAsync<T>(requestUri, content);
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
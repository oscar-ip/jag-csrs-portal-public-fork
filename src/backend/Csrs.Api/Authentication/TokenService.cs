using Csrs.Api.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace Csrs.Api.Authentication
{
    [Serializable]
    public class OAuthApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        public OAuthApiException(string message, int statusCode, string? response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception? innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + TrimToLength(response, 512), innerException)
        {
            StatusCode = statusCode;
            Response = response ?? string.Empty;
            Headers = headers;
        }

        public override string ToString()
        {
            return $"HTTP Response: \n\n{Response}\n\n{base.ToString()}";
        }

        private static string TrimToLength(string? value, int length)
        {
            if (value is null) return string.Empty;

            if (value.Length > length)
            {
                value = value[..length];
            }

            return value;
        }
    }

    public interface IOAuthApiClient
    {
        Task<Token?> GetRefreshToken(CancellationToken cancellationToken);
    }

    /// <summary>
    /// The OAuthApiClient interact with OAuth endpoing to manage refresh tokens.
    /// </summary>
    public class OAuthApiClient : IOAuthApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly OAuthConfiguration _configuration;

        public OAuthApiClient(HttpClient httpClient, OAuthConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Token?> GetRefreshToken(CancellationToken cancellationToken)
        {
            Dictionary<string, string> data = new()
            {
                {"resource", _configuration.ResourceUrl ?? string.Empty},
                {"client_id", _configuration.ClientId ?? string.Empty },
                {"client_secret", _configuration.Secret ?? string.Empty },
                {"username", _configuration.Username ?? string.Empty },
                {"password", _configuration.Password ?? string.Empty },
                {"scope", "openid"},
                {"response_mode", "form_post"},
                {"grant_type", "password"}
            };

            var content = new FormUrlEncodedContent(data);

            using var request = new HttpRequestMessage(HttpMethod.Post, _configuration.AuthorizationUrl) { Content = content };
            request.Headers.Add("client-request-id", Guid.NewGuid().ToString());
            request.Headers.Add("return-client-request-id", "true");
            request.Headers.Add("Accept", "application/json");

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var responseData = response.Content == null
                    ? null
                    : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                throw new OAuthApiException(
                    "The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").",
                    (int)response.StatusCode, responseData,
                    response.Headers.ToDictionary(x => x.Key, x => x.Value), null);
            }
            if (response.Content != null)
            {
                Token? token = await response.Content.ReadFromJsonAsync<Token>(cancellationToken: cancellationToken);
                return token;
            }

            return null; // will this happen?
        }
    }

    public interface ITokenService
    {
        Task<Token?> GetTokenAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// The TokenService expose services to get OAuth Tokens
    /// </summary>
    public class TokenService : ITokenService
    {
        private const int Buffer = 60;

        private const string token_key = "oauth-token";

        private readonly IOAuthApiClient _oAuthApiClient;
        private readonly IMemoryCache _cache;

        public TokenService(IOAuthApiClient oAuthApiClient, IMemoryCache cache)
        {
            _oAuthApiClient = oAuthApiClient ?? throw new ArgumentNullException(nameof(oAuthApiClient));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// Gets a token, from the cache or the authority provider.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Token?> GetTokenAsync(CancellationToken cancellationToken)
        {
            return await GetOrRefreshTokenAsync(cancellationToken);
        }

        private async Task<Token?> GetOrRefreshTokenAsync(CancellationToken cancellationToken)
        {
            Token? token = _cache.Get<Token>(token_key);
            if (token is null)
            {
                token = await RefreshTokenAsync(cancellationToken);
            }

            return token;
        }

        private async Task<Token?> RefreshTokenAsync(CancellationToken cancellationToken)
        {
            Token? token = await _oAuthApiClient.GetRefreshToken(cancellationToken);
            if (token != null)
            {
                var options = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(token.ExpiresIn - Buffer) };
                _cache.Set(token_key, token, options);
            }

            return token;
        }
    }
}

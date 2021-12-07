using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Csrs.Api.Configuration;

namespace Csrs.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IOptions<JwtBearerOptions> _options;
        private readonly IOptions<JwtAccessTokenConfiguration> _accessTokenConfiguration;
        private const string TokenEndpoint = "token_endpoint";
        private const string UserInfoEndpoint = "userinfo_endpoint";

        public AuthenticationController(IOptions<JwtBearerOptions> options, IOptions<JwtAccessTokenConfiguration> accessTokenConfiguration)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _accessTokenConfiguration = accessTokenConfiguration ?? throw new ArgumentNullException(nameof(accessTokenConfiguration));
        }

        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            string configuration = await GetBaseOpenidConfigurationAsync();

            JsonNode configurationNode = JsonNode.Parse(configuration);
            var endpoint = configurationNode[UserInfoEndpoint].ToString();

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;

            var header = Request.Headers.Authorization[0];
            string[] headers = header.Split(' ');

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(headers[0], headers[1]);


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);
            HttpResponseMessage? response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                string responseConent = await response.Content.ReadAsStringAsync();
                JsonNode content = JsonNode.Parse(responseConent);

                return Ok(content);
            }

            return BadRequest();

        }
        // token
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromForm] Dictionary<string, string> data)
        {
            data["client_secret"] = _accessTokenConfiguration.Value.ClientId;

            string configuration = await GetBaseOpenidConfigurationAsync();

            JsonNode configurationNode = JsonNode.Parse(configuration);
            var endpoint = configurationNode[TokenEndpoint].ToString();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);

            var content = new FormUrlEncodedContent(data);

            var request = new HttpRequestMessage() { Content = content };
            request.Method = HttpMethod.Post;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage? response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                string responseConent = await response.Content.ReadAsStringAsync();
                return Ok(responseConent);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest: 
                    return BadRequest();
            }

            return Ok();
        }

        [HttpGet(".well-known/openid-configuration")]
        public async Task<IActionResult> GetOpenidConfigurationAsync()
        {
            // get https://dev.oidc.gov.bc.ca/auth/realms/onestopauth-basic/.well-known/openid-configuration

            string configuration = await GetBaseOpenidConfigurationAsync();
            JsonNode configurationNode = JsonNode.Parse(configuration);

            // replace endpoints with our endpoints
            configurationNode[TokenEndpoint] = "authentication/token";
            configurationNode[UserInfoEndpoint] = "authentication/userinfo";

            return Ok(configurationNode);
        }

        private async Task<string?> GetBaseOpenidConfigurationAsync()
        {
            HttpClient client = new HttpClient(); // todo: use httpclient factory
            client.BaseAddress = new Uri(_options.Value.GetMetadataAddress());

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage? response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                string configuration = await response.Content.ReadAsStringAsync();
                return configuration;
            }

            return null;
        }
    }
}

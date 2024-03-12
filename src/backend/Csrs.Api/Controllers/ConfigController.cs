using AutoMapper.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csrs.Api.Controllers
{
    public class ConfigController : CsrsControllerBase<ConfigController>
    {
        private readonly IConfiguration _configuration;

        readonly List<(string, string)> features = new List<(string, string)>{
                ("IsLoginDisabled", "false")
            };

        public ConfigController(IMediator mediator, ILogger<ConfigController> logger, IConfiguration configuration)
            : base(mediator, logger)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("AppConfig")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public List<string> GetAppSettings()
        {
            var value = Environment.GetEnvironmentVariable("ISLOGINDISABLED");
            var activeFeatures = new List<string>();
            
            if (value != null)
            {
                activeFeatures.Add(value);
            }
            else
            {
                foreach (var feature in this.features)
                {
                    if (!string.IsNullOrEmpty(_configuration[feature.Item1]))
                    {
                        activeFeatures.Add(feature.Item2);
                    }
                }
            }

            
            return activeFeatures;
        }
    }
}

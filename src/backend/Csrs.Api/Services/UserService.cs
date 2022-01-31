using System.Security.Claims;

namespace Csrs.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetBCeIDUserId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is null)
            {
                _logger.LogInformation("HttpContext is null. Service may be been called outside of a api request");
                return string.Empty;
            }

            ClaimsPrincipal principal = context.User;

            Claim? userid = principal.Claims.SingleOrDefault(_ => _.Type == "bceid_userid");
            if (userid is null)
            {
                _logger.LogInformation("Current user does not have a bceid_userid claim");
                return string.Empty;
            }

            if (Guid.TryParse(userid.Value, out Guid id))
            {
                return id.ToString("d"); // format with dashes : xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
            }

            using var scope = _logger.AddProperty("bceid_userid", userid.Value);
            _logger.LogInformation("Current user's bceid_userid cannot be parsed as a valid guid");

            return string.Empty;
        }
    }
}

using System.Security.Claims;

namespace Csrs.Api.Models
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetBCeIDUserId(this ClaimsPrincipal principal)
        {
            Claim? userid = principal?.Claims?.SingleOrDefault(_ => _.Type == "bceid_userid");

            if (userid != null && Guid.TryParse(userid.Value, out Guid id))
            {
                return id;
            }

            return null;
        }
    }
}

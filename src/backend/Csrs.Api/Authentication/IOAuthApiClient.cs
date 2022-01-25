namespace Csrs.Api.Authentication
{
    public interface IOAuthApiClient
    {
        Task<Token?> GetRefreshToken(CancellationToken cancellationToken);
    }
}

namespace Csrs.Api.Authentication
{
    public interface ITokenService
    {
        Task<Token?> GetTokenAsync(CancellationToken cancellationToken);
    }
}

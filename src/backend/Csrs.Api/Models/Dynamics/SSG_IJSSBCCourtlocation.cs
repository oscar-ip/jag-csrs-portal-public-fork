namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_IJSSBCCourtlocation
    {
        public override Guid Key => CourtLocationId ?? Guid.Empty;
    }
}

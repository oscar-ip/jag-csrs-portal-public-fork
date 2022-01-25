namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_CsrsParty
    {
        public static readonly StatusCode<PartyStatus> Active = new StatusCode<PartyStatus>(1, "Active", PartyStatus.Active);

        public override Guid Key => PartyId ?? Guid.Empty;
    }
}

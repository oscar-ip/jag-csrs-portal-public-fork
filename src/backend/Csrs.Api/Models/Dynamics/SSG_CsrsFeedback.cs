namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_CsrsFeedback
    {
        public override Guid Key => FeedbackId ?? Guid.Empty;
    }
}

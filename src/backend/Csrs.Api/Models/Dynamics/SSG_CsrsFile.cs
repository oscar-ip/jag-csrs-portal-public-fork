namespace Csrs.Api.Models.Dynamics
{
    public partial class SSG_CsrsFile
    {
        public static readonly StatusCode<FileStatus> Active = new StatusCode<FileStatus>(1, "Active", FileStatus.Active);

        /// <summary>
        /// The party creating the file is the Payor.
        /// </summary>
        public static readonly int PartyEnrolledIsPayor = 867670000;

        /// <summary>
        /// The party creating the file is the Recipient.
        /// </summary>
        public static readonly int PartyEnrolledIsRecipient = 867670001;

        public override Guid Key => CsrsFileId ?? Guid.Empty;
  }
}

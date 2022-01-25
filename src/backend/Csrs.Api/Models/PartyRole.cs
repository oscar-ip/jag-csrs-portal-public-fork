namespace Csrs.Api.Models
{
    public enum PartyRole
    {
        /// <summary>
        /// The default party role.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The party is the recipient of support.
        /// </summary>
        Recipient = 1,

        /// <summary>
        /// The party is the payor of support.
        /// </summary>
        Payor
    }
}

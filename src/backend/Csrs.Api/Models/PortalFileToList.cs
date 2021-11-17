namespace Csrs.Api.Models
{
    public class PortalFileToList
    {
        public Guid FileGuid { get; set; }
        public string? UserRole { get; set; }
        public string? CSRSFileNumber { get; set; }
        public string? RegistrationDate { get; set; }
        public string? CourtFileNumber { get; set; }
        public string? StyleofCause { get; set; }
        public string? StatusReason { get; set; }
    }
}

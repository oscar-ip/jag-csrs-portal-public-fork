using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class NewFileRequest
    {
        /// <summary>
        /// This is a TEMPORARY field until login is implemented. The BCeiD Guid should come from the auth token
        /// </summary>
        [Obsolete("This is temporary field until authentication is complete")]
        [Required]
        public Guid BCeiD { get; set; }

        /// <summary>
        /// The details of the person making creating the file. User will be created if they do not exist.
        /// </summary>
        [Required]
        public Party? User { get; set; }

        /// <summary>
        /// The file to create.
        /// </summary>
        [Required]
        public File? File { get; set; }
    }
}

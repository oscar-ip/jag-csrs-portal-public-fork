using System.ComponentModel.DataAnnotations;

namespace Csrs.Api.Models
{
    public class NewFileRequest
    {
        
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

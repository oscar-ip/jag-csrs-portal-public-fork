using Swashbuckle.AspNetCore.Annotations;

namespace Csrs.Api.Models
{
    public class Child
    {
        /// <summary>
        /// The child id.
        /// </summary>
        public string? ChildId { get; set; }

        /// <summary>
        /// Child's first name.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Child's middle name.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Child's last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Child's date of birth name.
        /// </summary>
        [SwaggerSchema(Format = "date")]
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Child is a dependent.
        /// </summary>
        public string? ChildIsDependent { get; set; }
    }
}

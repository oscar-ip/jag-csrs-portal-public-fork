using System.Diagnostics;

namespace Csrs.Api.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class LookupValue
    {
        public LookupValue(int value, string text)
        {
            Id = value;
            Value = text;
        }

        public LookupValue()
        {
        }

        /// <summary>
        /// The unique id of the value.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The human readable value.
        /// </summary>
        public string? Value { get; set; }

        private string DebuggerDisplay => $"{Value} ({Id})";
    }
}


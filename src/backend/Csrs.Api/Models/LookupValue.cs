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
        public int Id { get; set; }

        /// <summary>
        /// The human readable value.
        /// </summary>
        public string Value { get; set; }

        private string DebuggerDisplay => $"{Value} ({Id})";
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CourtLookupValue
    {
        public CourtLookupValue(string guidId, string text)
        {
            Id = guidId;
            Value = text;
        }

        public CourtLookupValue()
        {
        }

        /// <summary>
        /// The unique id of the value.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The human readable value.
        /// </summary>
        public string Value { get; set; }

        private string DebuggerDisplay => $"{Value} ({Id})";
    }
}


using System.Diagnostics;

namespace Csrs.Api.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OptionValue
    {
        public OptionValue(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public int Value { get; set; }
        public string Text { get; set; }

        private string DebuggerDisplay => $"{Text} ({Value})";
    }
}

using System;
using System.Text;

namespace CrmSvcUtilExtensions
{
    internal class IndentingStringBuilder
    {
        private readonly StringBuilder sb = new StringBuilder();
        private int _indentLevel = 0;

        public void Indent()
        {
            _indentLevel++;
        }

        public void Unindent()
        {
            if (_indentLevel > 0)
            {
                _indentLevel--;
            }
            else
            {
                throw new InvalidOperationException("cannot unindent anymore");
            }
        }

        public void UnindentAndAppendLine(string value)
        {
            Unindent();
            AppendLine(value);
        }

        public void Append(string value, bool indent = true)
        {
            if (indent)
            {
                AppendIndent();
            }

            sb.Append(value);
        }

        public void AppendLine(string value, bool indent = true)
        {
            if (indent)
            {
                AppendIndent();
            }
            sb.AppendLine(value);
        }

        public void AppendLine()
        {
            sb.AppendLine();
        }


        public override string ToString() => sb.ToString();

        private void AppendIndent()
        {
            for (int i = 0; i < _indentLevel; i++)
            {
                sb.Append("    ");
            }
        }
    }  
}

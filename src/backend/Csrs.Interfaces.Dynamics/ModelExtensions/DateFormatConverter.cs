using Newtonsoft.Json.Converters;

namespace Csrs.Interfaces.Dynamics.Models
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}

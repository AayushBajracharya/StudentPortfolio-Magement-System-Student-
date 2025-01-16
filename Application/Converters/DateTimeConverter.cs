using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private string formatDate = "dd/MM/yyyy";
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), formatDate,
                CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(formatDate));
        }
    }

    public class TimeOnlyConverter : JsonConverter<DateTime>
    {
        private const string formatTime = "hh:mm tt";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeStr = reader.GetString();
            if (timeStr == null) return DateTime.MinValue;

            // Parse time string to TimeOnly first
            var time = TimeOnly.ParseExact(timeStr, formatTime, CultureInfo.InvariantCulture);

            // Convert to DateTime using the minimum date (we only care about the time part)
            return DateTime.Today.Add(time.ToTimeSpan());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(formatTime));
        }
    }


}
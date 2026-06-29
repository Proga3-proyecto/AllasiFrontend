using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Progra3_Frontend.Services
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                long unixTime = reader.GetInt64();
                return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).LocalDateTime;
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateStr = reader.GetString();
                if (!string.IsNullOrEmpty(dateStr) && dateStr.Contains("T") && dateStr.Length >= 19)
                {
                    dateStr = dateStr.Substring(0, 19);
                }
                if (DateTime.TryParse(dateStr, out DateTime date))
                {
                    return date;
                }
            }
            throw new JsonException("Unable to convert value to DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            }
            else
            {
                // Send without timezone offset to prevent backend from shifting the time
                writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
        }
    }

    public class CustomNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonTokenType.Number)
            {
                long unixTime = reader.GetInt64();
                return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).LocalDateTime;
            }
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateStr = reader.GetString();
                if (!string.IsNullOrEmpty(dateStr) && dateStr.Contains("T") && dateStr.Length >= 19)
                {
                    dateStr = dateStr.Substring(0, 19);
                }
                if (DateTime.TryParse(dateStr, out DateTime date))
                {
                    return date;
                }
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                if (value.Value.Kind == DateTimeKind.Utc)
                {
                    writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                }
                else
                {
                    writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
                }
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}

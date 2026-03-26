using System.Text.Json;
using System.Text.Json.Serialization;

namespace NCOSystems.WEB.Helpers
{
    public class StringToDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();

                if (string.IsNullOrWhiteSpace(strValue))
                    return null; // viene vacío -> retorna null

                if (DateTime.TryParse(strValue, out DateTime value))
                    return value;

                return null; // formato inválido -> retorna null
            }

            if (reader.TokenType == JsonTokenType.Null)
                return null;

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            else
                writer.WriteNullValue();
        }
    }
}

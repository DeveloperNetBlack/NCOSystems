using System.Text.Json;
using System.Text.Json.Serialization;

namespace NCOSystems.WEB.Helpers
{
    public class StringToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString();
                if (int.TryParse(strValue, out int value))
                    return value;

                return 0; // valor por defecto si viene vacío o inválido
            }

            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt32();

            return 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value);
    }
}

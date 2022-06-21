using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using DewIt.Model.DataTypes;

namespace DewIt.Model.Algorithms;

public class JsonURNConverter : JsonConverter<URN>
{
    public override URN? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        const string malformed = "Unable to convert URN. JSON is malformed.";

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException(malformed);
        }

        URN? urn = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return urn;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException(malformed); // reader broken?
            
            var propertyName = reader.GetString();
            if (propertyName != URN.AttributeName)
                throw new JsonException(
                    $"Unable to convert URN. Serialized URN must contain only the {URN.AttributeName} property.");
            
            // Get the value.
            reader.Read();
            var value = reader.GetString()!;
            urn = new URN(value);
        }

        throw new JsonException(malformed); // reader broken?
    }

    public override void Write(Utf8JsonWriter writer, URN value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(URN.AttributeName) ?? URN.AttributeName);
        writer.WriteStringValue(string.Format(value.NameString, CultureInfo.InvariantCulture));
        writer.WriteEndObject();
    }
}
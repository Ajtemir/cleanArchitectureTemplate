using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ukid.Domain.Enums;

public class EnumerationJsonConverter<T> : JsonConverter<T>
    where T : Enumeration
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        try
        {
            return Enumeration.FromDisplayName<T>(value);
        }
        catch (InvalidOperationException ex)
        {
            throw new JsonException(ex.Message);
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}

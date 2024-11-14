using System.Text.Json.Serialization;

namespace Ukid.Domain.Enums;

[JsonConverter(typeof(EnumerationJsonConverter<Gender>))]
public class Gender : Enumeration
{
    public Gender(int id, string name) : base(id, name)
    {
    }
    
    public static readonly Gender Male = new (1, nameof(Male));
    public static readonly Gender Female = new (2, nameof(Female));
}

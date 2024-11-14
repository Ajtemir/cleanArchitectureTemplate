using System.Text.Json.Serialization;

namespace Ukid.Domain.Enums;

[JsonConverter(typeof(EnumerationJsonConverter<DomainRole>))]
public class DomainRole : Enumeration
{
    public string Note { get; private set; }
    
    public DomainRole(int id, string name, string note) : base(id, name)
    {
        Note = note;
    }

    public static readonly DomainRole Administrator = new(1, nameof(Administrator), "Администратор");
    public static readonly DomainRole Manager = new(2, nameof(Manager), "Менеджер");
    public static readonly DomainRole User = new(3, nameof(User), "Сотрудник казино/службы охраны");
}

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ukid.Domain.Enums;

namespace Infrastructure.Common;

public class EnumerationValueConverter<T> : ValueConverter<T, string>
    where T : Enumeration
{
    public EnumerationValueConverter() : base(
        clrValue => clrValue.Name,
        dbValue => Enumeration.FromDisplayName<T>(dbValue))
    {
    }
}

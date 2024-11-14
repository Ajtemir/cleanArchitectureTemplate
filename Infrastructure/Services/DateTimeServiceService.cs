using Ukid.Application.Common.Interfaces;

namespace Infrastructure.Services;

public class DateTimeServiceService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}

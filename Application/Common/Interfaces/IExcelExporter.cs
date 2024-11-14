namespace Ukid.Application.Common.Interfaces;

public interface IExcelExporter
{
    Task<MemoryStream> ExportAsync<T>(IEnumerable<T> records,
        IEnumerable<int>? dateColumnIndexes = null,
        IEnumerable<int>? dateTimeColumnIndexes = null,
        string reportName = "Отчёт",
        CancellationToken cancellationToken = default);
}

using OfficeOpenXml;
using Ukid.Application.Common.Interfaces;

namespace Infrastructure.Services;

public class ExcelExporter : IExcelExporter
{
    public async Task<MemoryStream> ExportAsync<T>(IEnumerable<T> records,
        IEnumerable<int>? dateColumnIndexes = null,
        IEnumerable<int>? dateTimeColumnIndexes = null,
        string reportName = "Отчёт",
        CancellationToken cancellationToken = default)
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(reportName);
        FormatDateColumns(dateColumnIndexes, dateTimeColumnIndexes, ws);

        ws.Cells["A1"].LoadFromCollection(records, PrintHeaders: true).AutoFitColumns();

        var ms = new MemoryStream();
        await package.SaveAsAsync(ms, cancellationToken);
        ms.Position = 0;
        return ms;
    }

    private static void FormatDateColumns(IEnumerable<int>? dateColumnIndexes, IEnumerable<int>? dateTimeColumnIndexes, ExcelWorksheet ws)
    {
        if (dateColumnIndexes is null || dateTimeColumnIndexes is null)
        {
            return;
        }
        
        foreach (var index in dateColumnIndexes)
        {
            ws.Column(index).Style.Numberformat.Format = "dd/MM/yyyy";
        }

        foreach (var index in dateTimeColumnIndexes)
        {
            ws.Column(index).Style.Numberformat.Format = "dd/MM/yyyy hh:mm:ss";
        }
    }
}

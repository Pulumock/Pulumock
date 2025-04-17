using ClosedXML.Excel;
using Dataset.Clients.Responses;
using Dataset.Services;

namespace Dataset.Utils;

internal static class ExcelExporter
{
    public static void ExportToWorkbook(IEnumerable<GitHubMinerResult> results, string filePath)
    {
        using var workbook = new XLWorkbook();

        foreach (GitHubMinerResult result in results)
        {
            IXLWorksheet sheet = workbook.Worksheets.Add(result.Repository);
            
            sheet.Cell(1, 1).Value = "Title";

            int row = 2;
            foreach (GetGitHubIssuesAsyncResponse issue in result.Issues)
            {
                sheet.Cell(row, 1).Value = issue.Title;
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        workbook.SaveAs(filePath);
    }
}

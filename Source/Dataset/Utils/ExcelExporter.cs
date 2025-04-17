using System.Data;
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

            sheet.Cell(1, 1).Value = "Status";
            sheet.Cell(1,2).Value = "Url";
            sheet.Cell(1, 3).Value = "Title";
            sheet.Cell(1, 4).Value = "Body";

            int row = 2;
            foreach (GetGitHubIssuesAsyncResponse issue in result.Issues)
            {
                sheet.Cell(row, 1).Value = issue.State;
                sheet.Cell(row, 2).Value = issue.HtmlUrl;
                sheet.Cell(row, 3).Value = issue.Title;
                sheet.Cell(row, 4).Value = issue.Body;
                row++;
            }

            sheet.Columns().AdjustToContents();
        }

        workbook.SaveAs(filePath);
    }
}

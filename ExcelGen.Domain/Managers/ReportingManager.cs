using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Enums;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Managers
{
    public class ReportingManager : IReportingManager
    {
        private IPurchaseRepository _purchaseRepository;
        private IIncomeRepository _incomeRepository;
        private IAccessRepository _accessRepository;

        private Dictionary<int, int> _monthsFinalValues = new Dictionary<int, int>();

        public ReportingManager(IPurchaseRepository purchaseRepository, IIncomeRepository incomeRepository, IAccessRepository accessRepository)
        {
            _purchaseRepository = purchaseRepository;
            _incomeRepository = incomeRepository;
            _accessRepository = accessRepository;
        }
        public void FillWorkbook(ExcelWorkbook workBook, int year, string userId)
        {
            var sharerAccesses = _accessRepository.GetSharerAccesses(userId).Result;
            var sharerUserIds = sharerAccesses.Select(x => x.AuthorId).ToList();
            sharerUserIds.Add(userId);

            var purchases = _purchaseRepository.GetPurchasesByYear(year, sharerUserIds).Result;
            var incomes = _incomeRepository.GetIncomesByYear(year, sharerUserIds).Result;
            var reportingDTOs = MergePurchasesAndIncomes(purchases, incomes);

            ExcelWorksheet worksheet = workBook.Worksheets.Add(year.ToString());

            PopulateHeaders(worksheet);
            var finalIndex = PopulateData(worksheet, reportingDTOs.OrderBy(dto => dto.Month).ToList());
            PopulateSummary(worksheet, finalIndex);
            PrepareExcel(worksheet, finalIndex);
        }

        #region Private
        private void PopulateHeaders(ExcelWorksheet worksheet)
        {

            worksheet.Cells["A1"].Value = "Name";
            worksheet.Cells["B1"].Value = "Type";
            worksheet.Cells["C1"].Value = "Category";
            worksheet.Cells["D1"].Value = "Added Date";
            worksheet.Cells["E1"].Value = "January";
            worksheet.Cells["F1"].Value = "February";
            worksheet.Cells["G1"].Value = "March";
            worksheet.Cells["H1"].Value = "April";
            worksheet.Cells["I1"].Value = "May";
            worksheet.Cells["J1"].Value = "June";
            worksheet.Cells["K1"].Value = "July";
            worksheet.Cells["L1"].Value = "August";
            worksheet.Cells["M1"].Value = "September";
            worksheet.Cells["N1"].Value = "October";
            worksheet.Cells["O1"].Value = "November";
            worksheet.Cells["P1"].Value = "December";

            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.View.FreezePanes(2, 1);
        }

        private int PopulateData(ExcelWorksheet worksheet, List<ReportingDTO> reportingDTOs)
        {
            int currentRow = 2;
            var groupedByMonth = reportingDTOs.GroupBy(dto => dto.Month);
            foreach (var group in groupedByMonth)
            {
                var maxValue = group.Where(dto => dto.Type == ReportingConstants.INCOME_NAME).Any() ?
                    group.Where(dto => dto.Type == ReportingConstants.INCOME_NAME).Max(dto => dto.Price) : 0;
                var minValue = group.Where(dto => dto.Type == ReportingConstants.PURCHASE_NAME).Any() ? 
                    group.Where(dto => dto.Type == ReportingConstants.PURCHASE_NAME).Min(dto => dto.Price) : 0;
                foreach (var element in group)
                {
                    worksheet.Cells[currentRow, 1].Value = element.Name;
                    worksheet.Cells[currentRow, 2].Value = element.Type;
                    worksheet.Cells[currentRow, 3].Value = element.Category;
                    worksheet.Cells[currentRow, 4].Value = element.AddedDate;
                    worksheet.Cells[currentRow, 4].Style.Numberformat.Format = "mm/dd/yyyy h:mm";
                    worksheet.Cells[currentRow, 4 + group.Key].Value = element.Price;
                    worksheet.Cells[currentRow, 4 + group.Key].Style.Numberformat.Format = "₴#,##0.00";

                    if (maxValue == element.Price && element.Type == ReportingConstants.INCOME_NAME)
                        worksheet.Cells[currentRow, 4 + group.Key].Style.Font.Color.SetColor(Color.Green);
                    else if (minValue == element.Price && element.Type == ReportingConstants.PURCHASE_NAME)
                        worksheet.Cells[currentRow, 4 + group.Key].Style.Font.Color.SetColor(Color.Red);

                    currentRow++;
                }

                var sum = group.Sum(el => el.Price);
                worksheet.Cells[currentRow, 1].Value = "Summary";
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                worksheet.Cells[currentRow, 4 + group.Key].Value = sum;
                worksheet.Cells[currentRow, 4 + group.Key].Style.Fill.SetBackground(sum > 0 ? Color.LightGreen : Color.FromArgb(218, 150, 148));
                worksheet.Cells[currentRow, 4 + group.Key].Style.Font.Bold = true;
                worksheet.Cells[currentRow, 4 + group.Key].Style.Numberformat.Format = "₴#,##0.00";

                _monthsFinalValues.TryAdd(group.Key, sum);

                currentRow++;
            }
            return currentRow;
        }

        private List<ReportingDTO> MergePurchasesAndIncomes(List<Purchase> purchases, List<Income> incomes)
        {
            var reportingDTOs = new List<ReportingDTO>();

            foreach (var purchase in purchases)
            {
                var dto = new ReportingDTO();
                dto.Name = purchase.Name;
                dto.Type = ReportingConstants.PURCHASE_NAME;
                dto.Category = purchase.Category.Name;
                dto.AddedDate = purchase.AddedDate;
                dto.Price = -purchase.Price;
                dto.Month = purchase.Month;

                reportingDTOs.Add(dto);
            }

            foreach (var income in incomes)
            {
                var dto = new ReportingDTO();
                dto.Name = income.Name;
                dto.Type = ReportingConstants.INCOME_NAME;
                dto.Category = income.Category.Name;
                dto.AddedDate = income.AddedDate;
                dto.Price = income.Price;
                dto.Month = income.Month;

                reportingDTOs.Add(dto);
            }

            return reportingDTOs;
        }

        private void PopulateSummary(ExcelWorksheet worksheet, int finalRowIndex)
        {
            worksheet.Cells[$"A{finalRowIndex}"].Value = "Year Summary:";

            foreach (var month in Enum.GetValues(typeof(eMonths)))
            {
                _monthsFinalValues.TryGetValue((int)month, out int value);
                worksheet.Cells[finalRowIndex, 4 + (int)month].Value = value;
                worksheet.Cells[finalRowIndex, 4 + (int)month].Style.Numberformat.Format = "₴#,##0.00";
            }
            worksheet.Row(finalRowIndex).Style.Font.Bold = true;
            finalRowIndex++;
            worksheet.Cells[$"A{finalRowIndex}"].Value = "Month Number:";
            foreach (var month in Enum.GetValues(typeof(eMonths)))
            {
                worksheet.Cells[finalRowIndex, 4 + (int)month].Value = (int)month;
            }

            ExcelChart chart = worksheet.Drawings.AddChart("FindingsChart",
OfficeOpenXml.Drawing.Chart.eChartType.Line);
            chart.Title.Text = "Year Budget Report";
            chart.SetPosition(finalRowIndex + 3, 0, 1, 0);
            chart.SetSize(800, 300);
            var ser1 = (ExcelChartSerie)(chart.Series.Add(worksheet.Cells[finalRowIndex - 1, 5, finalRowIndex - 1, 17], 
                worksheet.Cells[finalRowIndex, 5, finalRowIndex, 17]));
            ser1.Header = "Money";
        }

        private void PrepareExcel(ExcelWorksheet worksheet, int finalRowIndex)
        {
            worksheet.Cells[$"A1:P{finalRowIndex}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            worksheet.Cells[worksheet.Dimension.Address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[worksheet.Dimension.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
        #endregion
    }
}

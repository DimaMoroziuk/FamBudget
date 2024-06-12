using System;

namespace ExcelGen.Domain.DTOs
{
    public class ReportingDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public DateTime AddedDate { get; set; }
        public int Price { get; set; }
        public int Month { get; set; }
    }

    public static class ReportingConstants 
    {
        public static string INCOME_NAME = "Income";
        public static string PURCHASE_NAME = "Purchase";
    }
}

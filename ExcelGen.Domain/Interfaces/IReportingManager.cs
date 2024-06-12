using OfficeOpenXml;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Interfaces
{
    public interface IReportingManager
    {
        void FillWorkbook(ExcelWorkbook workBook, int year, string userId);
    }
}

using ExcelGen.Domain.Enums;
using ExcelGen.Repository.Models;

namespace ExcelGen.Domain.DTOs
{
    public class PurchaseDTO : Purchase
    {
        public PurchaseDTO(Purchase purchase)
        {
            Id = purchase.Id;
            Name = purchase.Name;
            AddedDate = purchase.AddedDate;
            Price = purchase.Price;
            Month = purchase.Month;
            Year = purchase.Year;
            Category = purchase.Category;
            CategoryId = purchase.CategoryId;
            Author = purchase.Author;
            AuthorId = purchase.AuthorId;
        }

        public eMonths MonthEnum
        {
            get { return (eMonths)Month; }
        }
        public int AccessType { get; set; }
    }
}

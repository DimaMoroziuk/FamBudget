using Microsoft.AspNetCore.Identity;
using System;

namespace ExcelGen.Repository.AuthorizationData
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

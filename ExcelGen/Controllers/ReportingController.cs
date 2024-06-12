using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExcelGen.Domain.DTOs;
using ExcelGen.Repository.Models;
using ExcelGen.Domain.Interfaces;
using System.Linq;
using System.Drawing;
using System.Net.Http;
using System.IO;
using OfficeOpenXml;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExcelGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportingController : ControllerBase
    {
        private IReportingManager _reportingManager;

        public ReportingController(IReportingManager reportingManager)
        {
            _reportingManager = reportingManager;
        }

        [HttpGet("getReport")]
        public async Task<ActionResult> DownloadExcelEPPlus(int year)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var stream = new MemoryStream();
            ExcelPackage excelPackage = new ExcelPackage(stream);
            _reportingManager.FillWorkbook(excelPackage.Workbook, year, userId);

            byte[] bytes = excelPackage.GetAsByteArray();
            string excelName = $"FamBudget-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = excelName,
                Inline = false
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("excelGenSaved")]
        public ActionResult SaveExcelEPPlus(int year)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var stream = new MemoryStream();
            ExcelPackage excelPackage = new ExcelPackage(stream);
            _reportingManager.FillWorkbook(excelPackage.Workbook, year, userId);

            byte[] bytes = excelPackage.GetAsByteArray();
            string excelName = $"FamBudget-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            string path = $@"D:\{excelName}";
            System.IO.File.WriteAllBytes(path, bytes);
            return Ok();
        }
    }
}

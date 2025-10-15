using System.Data;
using Microsoft.AspNetCore.Mvc;
using ExcelDataReader;
using AttendanceManagementPayrollSystem.Services;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/clockin")]
    public class ClockinController : ControllerBase
    {
        private readonly ClockinService _clockinService;

        public ClockinController(ClockinService clockinService)
        {
            _clockinService = clockinService;
        }

        [HttpGet("employee/{empId}")]
        public async Task<IActionResult> GetClockinsByEmployee(int empId, int month, int year)
        {
            if (month < 1 || month > 12) return BadRequest("Tháng không hợp lệ.");

            var data = await _clockinService.GetClockinsByEmployeeAsync(empId, month, year);
            if (data == null || data.Count == 0) return NotFound("Không tìm thấy dữ liệu.");

            return Ok(data);
        }

        [HttpPost("uploadxls")]
        public async Task<IActionResult> UploadXls()
        {
            if (Request.ContentLength == null || Request.ContentLength == 0)
                return BadRequest("No file content received");

            using var mem = new MemoryStream();
            await Request.Body.CopyToAsync(mem);
            mem.Position = 0;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var reader = ExcelReaderFactory.CreateBinaryReader(mem);
            var ds = reader.AsDataSet();

            if (ds.Tables.Count < 3) return BadRequest("Sheet 3 not found");

            var sheet3 = ds.Tables[2];

            //.ProcessSheet3(sheet3);
            foreach (DataRow row in sheet3.Rows)
            {
                var rowText = string.Join(", ", row.ItemArray.Select(cell => cell?.ToString() ?? ""));
                Console.WriteLine(rowText);
            }

            return Ok("Sheet 3 processed successfully");
        }
    }
}

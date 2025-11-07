using System.Data;
using Microsoft.AspNetCore.Mvc;
using ExcelDataReader;
using AttendanceManagementPayrollSystem.DTO;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using AttendanceManagementPayrollSystem.Services.ServiceList;

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

        [HttpGet("employee")]
        public async Task<IActionResult> GetClockinsByEmployee([FromQuery] int empId, [FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Tháng không hợp lệ.");

            var data = await _clockinService.GetClockinsByEmployeeAsync(empId, month, year);
            if (data == null) return NotFound("Không tìm thấy dữ liệu.");

            return Ok(data);
        }

        [HttpGet("employee/period")]
        public async Task<ActionResult<IEnumerable<ClockinDTO>>> GetClockIns(
            [FromQuery] int employeeId,
            [FromQuery] DateTime startDate,
            [FromQuery] int months)
        {
            if (months <= 0)
                return BadRequest("Months must be greater than 0.");

            var result = await _clockinService.GetByEmployeeAsync(employeeId, startDate, months);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("uploadxls")]
        public async Task<ActionResult<List<ClockinDTO>>> UploadXls()
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
            
            var data = await _clockinService.ReadClockinData(ds.Tables[2]);

            if (data == null || data.Count == 0) return NotFound("Không tìm thấy dữ liệu.");

            return Ok(data);
        }

        [HttpPost("upload-clockin")]
        public async Task<IActionResult> UploadClockins([FromBody] List<ClockinDTO> clockins)
        {
            if (clockins == null || clockins.Count == 0)
                return BadRequest("No clock-in data received.");

            try
            {
                await _clockinService.SaveClockinData(clockins);
                return Ok("Clockins saved");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                //_logger.LogError(ex, "Database update failed");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);
                //_logger.LogError(ex, "Unexpected error");
                return StatusCode(500, "Unexpected error");
            }
        }
    }
}

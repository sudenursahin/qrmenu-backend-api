using Azure.Core;
using Business.DTOs.Table;
using Business.GenericRepository.BaseRep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class TableController : Controller
{
    private readonly ITableService _tableService;
    private readonly ITableItemService _tableItemService;

    public TableController(
        ITableService tableService,
        ITableItemService tableItemService)
    {
        _tableService = tableService;
        _tableItemService = tableItemService;
    }

    // Get all tables
    [HttpGet]
    public async Task<IActionResult> GetAllTables()
    {
        var tables = await _tableService.GetAllTablesAsync();
        return Ok(tables);
    }

    // Get table by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTableById(int id)
    {
        var table = await _tableService.GetTableByIdAsync(id);
        return Ok(table);
    }

    // Create a new table
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] TableCreateDto tableCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _tableService.CreateTableAsync(tableCreateDto);
            return CreatedAtAction(nameof(GetTableById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            // Hata detaylarını logla
            Console.WriteLine($"Error in CreateTable: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }

            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Update table
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTable(int id, TableUpdateDto tableUpdateDto)
    {
        try
        {
            if (id != tableUpdateDto.Id)
                return BadRequest("Id mismatch");

            var table = await _tableService.UpdateTableAsync(id, tableUpdateDto);
            return Ok(table);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Delete table
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTable(int id)
    {
        var result = await _tableService.DeleteTableAsync(id);
        return Ok(result);
    }

    // Get QR code for specific table
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}/qr")]
    public async Task<IActionResult> GetTableQR(int id)
    {
        try
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var qrCodeBytes = await _tableService.GenerateTableQRCodeAsync(id, baseUrl);
            return File(qrCodeBytes, "image/png", $"masa-{id}-qr.png");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // Get all tables' QR codes
    [Authorize(Roles = "Admin")]
    [HttpGet("qr-codes/all")]
    public async Task<IActionResult> GetAllTableQRCodes()
    {
        try
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var zipBytes = await _tableService.GetAllTableQRCodesZipAsync(baseUrl);
            return File(zipBytes, "application/zip", "tum-masa-qr-kodlari.zip");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
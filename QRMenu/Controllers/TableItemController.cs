using Business.DTOs.MenuItem;
using Business.DTOs.TableItem;
using Business.GenericRepository.BaseRep;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace QRMenu.Controllers
{
    [ApiController]
    [Route("api/tables")]  // Route'u daha RESTful yaptık
    public class TableItemController : ControllerBase
    {
        private readonly ITableItemService _tableItemService;
        private readonly ITableService _tableService;
        private readonly ILogger<TableItemController> _logger;

        public TableItemController(ITableItemService tableItemService, ILogger<TableItemController> logger, ITableService tableService)
        {
            _tableItemService = tableItemService;
            _logger = logger;
            _tableService = tableService;
        }

        [HttpGet("table/{tableId:int}")]
        public async Task<IActionResult> GetTableItems(int tableId)
        {
            try
            {
                var items = await _tableItemService.GetTableItemsByTableIdAsync(tableId);
                return Ok(items ?? new List<TableItemGetDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving table items for tableId: {TableId}", tableId);
                throw; // Let the global exception handler deal with it
            }
        }


        [HttpPost("{tableId}/items")]
        public async Task<IActionResult> AddTableItem(int tableId, [FromBody] TableItemCreateDto tableItemCreateDto)
        {
            try
            {
                _logger.LogInformation($"Adding item to table {tableId}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (tableId != tableItemCreateDto.TableId)
                {
                    _logger.LogWarning($"TableId mismatch. URL: {tableId}, DTO: {tableItemCreateDto.TableId}");
                    return BadRequest("TableId in the URL does not match the TableId in the request body.");
                }

                var response = await _tableItemService.CreateTableItemAsync(tableId, tableItemCreateDto);
                _logger.LogInformation($"Successfully added item to table {tableId}");

                return CreatedAtAction(
                    nameof(GetTableItems),
                    new { tableId = response.TableId },
                    response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Resource not found while adding item to table {tableId}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding item to table {tableId}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{tableId}/items/{itemId}")]
        public async Task<IActionResult> UpdateTableItem(
      int tableId,
      int itemId,
      [FromBody] TableItemUpdateDto tableItemUpdateDto)
        {
            try
            {
                _logger.LogInformation($"Updating item {itemId} for table {tableId}");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Invalid model state for item {itemId}, table {tableId}");
                    return BadRequest(ModelState);
                }

                // Null check for DTO
                if (tableItemUpdateDto == null)
                {
                    return BadRequest("Request body cannot be null");
                }

                // ID validation
                if (tableId != tableItemUpdateDto.TableId || itemId != tableItemUpdateDto.Id)
                {
                    _logger.LogWarning($"ID mismatch in update request. URL: {tableId}/{itemId}, DTO: {tableItemUpdateDto.TableId}/{tableItemUpdateDto.Id}");
                    return BadRequest("IDs in the URL do not match the IDs in the request body.");
                }

                // Metod çağrısını UpdateTableItemAsync olarak değiştirdik
                var response = await _tableItemService.UpdateTableItemAsync(tableId, itemId, tableItemUpdateDto);
                _logger.LogInformation($"Successfully updated item {itemId} for table {tableId}");
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Validation error while updating item {itemId} for table {tableId}");
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Resource not found while updating item {itemId} for table {tableId}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating item {itemId} for table {tableId}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{tableId}/items/{tableItemId}")]
        public async Task<IActionResult> DeleteTableItem(int tableId, int tableItemId)
        {
            try
            {
                _logger.LogInformation($"Deleting item {tableItemId} from table {tableId}");

                var result = await _tableItemService.DeleteTableItemAsync(tableId, tableItemId);

                if (!result)
                {
                    return NotFound($"Table item with ID {tableItemId} not found for table {tableId}");
                }

                _logger.LogInformation($"Successfully deleted item {tableItemId} from table {tableId}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Resource not found while deleting item {tableItemId} from table {tableId}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting item {tableItemId} from table {tableId}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
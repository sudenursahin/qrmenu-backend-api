using Business.DTOs.Order;
using Business.DTOs.TableItem;
using Business.GenericRepository.BaseRep;
using CoreL.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QRMenu.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
          _orderService = orderService;
        }



        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus newStatus)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, newStatus);
                return Ok(updatedOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpGet("table/{tableId}")]
        public async Task<IActionResult> GetOrdersByTableId(int tableId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByTableIdAsync(tableId);
                return Ok(orders);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("table/{tableId}/total")]
        public async Task<IActionResult> CalculateTableTotal(int tableId)
        {
            try
            {
                var total = await _orderService.CalculateTableTotalAsync(tableId);
                return Ok(new { TableId = tableId, Total = total });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Table with id {tableId} not found or has no items");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while calculating the table total");
            }
        }

    }
}






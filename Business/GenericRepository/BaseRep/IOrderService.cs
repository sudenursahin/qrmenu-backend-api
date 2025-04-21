using Business.DTOs.Order;
using CoreL.Enums;
using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.GenericRepository.BaseRep
{
    public interface IOrderService
    {
        Task<OrderGetDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderGetDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderGetDto>> GetAllOrdersAsync();
        Task<OrderGetDto> UpdateOrderStatusAsync(int id, OrderStatus newStatus);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<OrderGetDto>> GetOrdersByTableIdAsync(int id);
        Task<decimal> CalculateTableTotalAsync(int tableId);
    }
    
}
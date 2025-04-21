using AutoMapper;
using Business.DTOs.Order;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcRep;
using CoreL.Enums;
using Domain;

namespace Business.GenericRepository.ConcManager
{
    public class OrderManager : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TableRepository _tableRepository;
        private readonly TableItemRepository _tableItemRepository;
        private readonly MenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public OrderManager(OrderRepository orderRepository, TableRepository tableRepository, IMapper mapper, TableItemRepository tableItemRepository, MenuItemRepository menuItemRepository)
        {
            _orderRepository = orderRepository;
            _tableRepository = tableRepository;
            _mapper = mapper;
            _tableItemRepository = tableItemRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<OrderGetDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var table = await _tableRepository.GetByIdAsync(createOrderDto.TableId);
            if (table == null)
                throw new KeyNotFoundException($"Table with id {createOrderDto.TableId} not found.");

            if (!table.TableItems.Any())
                throw new InvalidOperationException($"Table {createOrderDto.TableId} has no items to create an order.");

            var order = new Order
            {
                TableId = createOrderDto.TableId,
                Status = OrderStatus.Preparing,
                OrderDate = DateTime.UtcNow,
                OrderItems = table.TableItems.Select(ti => new OrderItem
                {
                    MenuItemId = ti.MenuItemId,
                    Quantity = ti.Quantity,
                    Price = ti.MenuItem.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            // Clear the table items after creating the order
            table.TableItems.Clear();
            table.Status = TableStatus.Occupied;
            await _tableRepository.UpdateAsync(table);

            // Siparişi tekrar çekip dönüştürün
            var createdOrder = await _orderRepository.GetByIdAsync(order.Id);
            return _mapper.Map<OrderGetDto>(createdOrder);
        }

        public async Task<OrderGetDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found.");

            return _mapper.Map<OrderGetDto>(order);
        }

        public async Task<IEnumerable<OrderGetDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllItems();
            return _mapper.Map<IEnumerable<OrderGetDto>>(orders);
        }

        public async Task<OrderGetDto> UpdateOrderStatusAsync(int id, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found.");

            order.Status = newStatus;
            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderGetDto>(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            await _orderRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<OrderGetDto>> GetOrdersByTableIdAsync(int tableId)
        {
            var orders = await _orderRepository.GetOrdersByTableIdAsync(tableId);
            return _mapper.Map<IEnumerable<OrderGetDto>>(orders);
        }



        public async Task<decimal> CalculateTableTotalAsync(int tableId)
        {
            var table = await _tableRepository.GetByIdAsync(tableId);
            if (table == null)
            {
                throw new KeyNotFoundException($"Table with id {tableId} not found");
            }

            var tableItems = await _tableItemRepository.GetTableItemsByTableIdAsync(tableId);
            if (!tableItems.Any())
            {
                return 0;
            }

            // MenuItem null kontrolü ekleyelim
            var total = tableItems
                .Where(item => item.MenuItem != null)
                .Sum(item => item.MenuItem.Price * item.Quantity);

            return total;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class OrderService : IOrderService
    {
        ApplicationDbContext dbContext;
        public OrderService(ApplicationDbContext Context)
        {
            dbContext = Context;
        }
        public async Task<Order> GetOrder()
        { 
            //var res = dbContext.Orders.MaxBy(o => o.Price * o.Quantity);
            //return res;

            var resultOrder = from order in dbContext.Orders
                              where order.Price * order.Quantity == (from or in dbContext.Orders
                                                                     select or.Quantity*or.Price).Max()
                              select order;
            
            
            return await resultOrder.FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrders()
        {
            return await dbContext.Orders.Where(q => q.Quantity > 10).ToListAsync();
        }
    }
}

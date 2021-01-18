using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Models;
using Microsoft.EntityFrameworkCore;

namespace PetStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private DatabaseContext _context;

        public OrderRepository(DatabaseContext ctx) => _context = ctx;

        public IQueryable<Order> Orders => _context.Orders
            .Include(x => x.Lines)
                .ThenInclude(x => x.Article);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(x => x.Article));
            if(order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}

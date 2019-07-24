using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var orders = _db.Orders.ToList();
            var title = "Orders";
            var model = new OrderIndexViewModel
            {
                Orders = orders,
                Title = title
            };
            return View(model);
        }
        public IActionResult New()
        {
            var orders = _db.Orders.Where(o => o.IsDelivered == false).ToList();
            var title = "New Orders";
            var model = new OrderIndexViewModel
            {
                Orders = orders,
                Title = title
            };
            return View("Index",model);
        }
        public IActionResult Delivered()
        {
            var orders = _db.Orders.Where(o => o.IsDelivered == true).ToList();
            var title = "Orders Completed";
            var model = new OrderIndexViewModel
            {
                Orders = orders,
                Title = title
            };
            return View("Index",model);
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var bookOrders = await _db.BookOrders.Include(bo => bo.Book).
                            ThenInclude(b => b.Author).Where(bo => bo.OrderId == id).ToListAsync();
            var model = new OrderViewModel
            {
                Order = order,
                BookOrders = bookOrders
            };
            return View(model);
        }
        public async Task<IActionResult> Deliver(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            order.IsDelivered = true;
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(New));
        }
    }
}
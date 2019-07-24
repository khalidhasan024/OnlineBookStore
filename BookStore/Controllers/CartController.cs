using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Extensions;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            this._db = db;
            this._userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<int> CartItems = HttpContext.Session.Get<List<int>>("Cart");
            List<Book> books = new List<Book>();
            if(CartItems!= null && CartItems.Count > 0)
            {
                foreach(var id in CartItems)
                {
                    var book = _db.Books.Include(b => b.Category).Include(b=>b.Author).Where(b => b.BookId == id).FirstOrDefault();
                    books.Add(book);
                }
            }
            return View(books);
        }
        public IActionResult Remove(int id)
        {
            List<int> CartItems = HttpContext.Session.Get<List<int>>("Cart");
            if (CartItems.Contains(id))
            {
                CartItems.Remove(id);
            }
            HttpContext.Session.Set("Cart", CartItems);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Order()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Order(NewOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = _userManager.GetUserId(HttpContext.User); ;
                Order order = new Order
                {
                    Date = DateTime.Today,
                    Name = model.Name,
                    Phone = model.Phone,
                    Address = model.Address,
                    IsDelivered = false,
                    UserId = _userManager.GetUserId(HttpContext.User)
                };
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                List<int> CartItems = HttpContext.Session.Get<List<int>>("Cart");
                if (CartItems != null)
                {
                    foreach (var bookId in CartItems)
                    {
                        var book = _db.Books.Find(bookId);
                        BookOrder bookOrder = new BookOrder
                        {
                            Book = book,
                            Order = order
                        };
                        _db.BookOrders.Add(bookOrder);
                    }
                    /*var book = _db.Books.Find(CartItems.FirstOrDefault());
                    BookOrder bookOrder = new BookOrder
                    {
                        Book = book,
                        Order = order
                    };
                    _db.BookOrders.Add(bookOrder);*/
                    await _db.SaveChangesAsync();

                    HttpContext.Session.Set("Cart", new List<int>());
                }

                return RedirectToAction("ThankYou","Home", new { id = order.Id});
            }
            return View(model);
        }
        
    }
}
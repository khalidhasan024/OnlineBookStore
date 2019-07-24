using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Extensions;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            var Books = _db.Books.Include(b => b.Category).Include(b => b.Author).OrderBy(b => b.Title);
            return View(Books);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _db.Books.Include(b => b.Category).Include(b => b.Author)
                        .SingleOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult AddToCart(int id)
        {
            List<int> CartItems = HttpContext.Session.Get<List<int>>("Cart");
            if(CartItems == null)
            {
                CartItems = new List<int>();
            }
            CartItems.Add(id);
            HttpContext.Session.Set("Cart", CartItems);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int id)
        {
            List<int> CartItems = HttpContext.Session.Get<List<int>>("Cart");
            if(CartItems.Contains(id))
            {
                CartItems.Remove(id);
            }
            HttpContext.Session.Set("Cart", CartItems);

            return RedirectToAction(nameof(Index));
        }
    }
}
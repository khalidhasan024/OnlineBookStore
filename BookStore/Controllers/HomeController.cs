using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookStore.Models.ViewModels;
using BookStore.Models.Repositories;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _repository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext db,
                            IBookRepository repository,
                            RoleManager<IdentityRole> roleManager,
                            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _repository = repository;
            _roleManager = roleManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            
            if(User.IsInRole("Admin"))
            {
                return RedirectToAction("New", "Order", new { area = "Admin" });
            }
            var Books = _repository.GetAllBooksWithDetails();
            return View(Books);
        }

        [HttpGet]
        [Route("/ThankYou/{id}")]
        public async Task<IActionResult> ThankYou(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if(order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            List<BookOrder> bookOrders = await _db.BookOrders.
                                        Include(bo => bo.Book).
                                        ThenInclude(b => b.Author).
                                        Where(bo => bo.OrderId == id).ToListAsync();
            var user = await _userManager.FindByIdAsync(userId);
            if(userId == order.UserId)
            {
                var model = new OrderViewModel
                {
                    Order = order,
                    User = user,
                    BookOrders = bookOrders
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<string> CreateRole()
        {
            if (await _roleManager.RoleExistsAsync("Admin"))
            {
                return "NOTHING TO SHOW !!!";
            }
            else
            {
                IdentityRole AdminRole = new IdentityRole { Name = "Admin" };
                var AdminRoleCreate = await _roleManager.CreateAsync(AdminRole);

                if (AdminRoleCreate.Succeeded)
                {
                    IdentityUser Admin = new IdentityUser
                    {
                        UserName = "admin@khalid.info",
                        Email = "admin@khalid.info"
                    };
                    var AdminCreate = await _userManager.CreateAsync(Admin, "Admin@2019");
                    if (AdminCreate.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(Admin, "Admin");

                        IdentityRole UserRole = new IdentityRole { Name = "User" };
                        await _roleManager.CreateAsync(UserRole);

                        return "ADMIN ROLE CREATED SUCCESSFULLY !";
                    }
                }
                return "ADMIN ROLE COULD NOT BE CREATED !";
            }
        }
        
    }
}

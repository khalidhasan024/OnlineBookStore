using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _repository;

        public AuthorController(ApplicationDbContext db, IBookRepository repository)
        {
            this._db = db;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var Authors = _repository.GetAllAuthorsWithDetails();
            return View(Authors);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var Author = _repository.GetAuthorDetailsById(id);
            if (Author == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            return View(Author);
        }
    }
}
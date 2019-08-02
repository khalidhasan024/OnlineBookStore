using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using Microsoft.EntityFrameworkCore;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using BookStore.Models.Repositories;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _repository;
        private readonly HostingEnvironment _hostingEnvironment;


        public BookViewModel ViewModel { get; set; }
        public BookController(ApplicationDbContext db, IBookRepository repository, HostingEnvironment host)
        {
            _db = db;
            _repository = repository;
            _hostingEnvironment = host;
            ViewModel = new BookViewModel()
            {
                Book = new Models.Book(),
                Authors = _repository.GetAllAuthors(),
                BookCategories = _repository.GetAllCategories()
                //Tags = _db.Tags.ToList()
            };
        }
        public IActionResult Index()
        {
            var books = _repository.GetAllBooksWithDetails();
            return View(books);
        }

        public IActionResult Create()
        {
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewModel.Book = model.Book;
                return View(ViewModel);
            }

            //_db.Books.Add(model.Book);
            _repository.AddBook(model.Book);
            //await _db.SaveChangesAsync();

            var RootDirectory = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                var filePath = Path.Combine(DataContext.ImageDirectory, model.Book.BookId + extension);
                using (var fileStream = new FileStream(Path.Combine(RootDirectory, filePath), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                model.Book.ImagePath = DataContext.ImageSource + model.Book.BookId + extension;
                
            }
            else
            {
                //_db.Attach(model.Book);
                model.Book.ImagePath = DataContext.ImageSource + DataContext.DefaultImage;
            }
            //await _db.SaveChangesAsync();
            _repository.UpdateBook(model.Book);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }

            /*var book = _db.Books.Include(b => b.Category)
                        .Include(b => b.Author)
                        .SingleOrDefault(b => b.BookId == id);*/
            var book = _repository.GetBookById(id);

            if (book == null)
            {
                return View("~/Views/NotFound.cshtml");
            }

            BookViewModel model = new BookViewModel();

            model.Book = book;
            model.Authors = _db.Authors.ToList();
            model.BookCategories = _db.BookCategories.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = model.Book;
                var files = HttpContext.Request.Form.Files;
                if (files.Any())
                {
                    var RootDirectory = _hostingEnvironment.WebRootPath;
                    var ext = Path.GetExtension(Path.GetFileName(book.ImagePath));
                    var img = Path.Combine(RootDirectory, DataContext.ImageDirectory, book.BookId + ext);

                    if (System.IO.File.Exists(img))
                    {
                        System.IO.File.Delete(img);
                    }

                    var extension = Path.GetExtension(files[0].FileName);
                    var filePath = Path.Combine(DataContext.ImageDirectory, model.Book.BookId + extension);
                    using (var fileStream = new FileStream(Path.Combine(RootDirectory, filePath), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    book.ImagePath = DataContext.ImageSource + model.Book.BookId + extension;
                }
                book.AuthorId = model.Book.AuthorId;
                book.CategoryId = model.Book.CategoryId;
                book.Discount = model.Book.Discount;
                book.Price = model.Book.Price;
                book.Stock = model.Book.Stock;
                book.Title = model.Book.Title;

                _repository.UpdateBook(book);
                return RedirectToAction(nameof(Index));
            }

            return View(model);

        }

        
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            var book = _repository.GetBookWithDetails(id);
            if(book == null)
            {
                return View("~/Views/NotFound.cshtml");
            }

            return View(book);
        }


        public IActionResult Delete(int? id)
        {
            if(id == null)
            {
                return View("~/Views/NotFound.cshtml");
            }
            /*var book = await _db.Books.Include(b => b.Category).Include(b => b.Author)
                        .Where(b => b.BookId == id).FirstOrDefaultAsync();*/
            var book = _repository.GetBookById(id);
            if(book == null)
            {
                return View("~/Views/NotFound.cshtml");
            }

            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteBook(Book book)
        {
            var RootDirectory = _hostingEnvironment.WebRootPath;
            var extension = Path.GetExtension(Path.GetFileName(book.ImagePath));
            var img = Path.Combine(RootDirectory, DataContext.ImageDirectory, book.BookId + extension);

            if (System.IO.File.Exists(img))
            {
                System.IO.File.Delete(img);
            }
            _repository.DeleteBook(book);
            return RedirectToAction(nameof(Index));
        }


        /*[HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = _db.Books.Include(b => b.Category)
                        .Include(b => b.Author)
                        .SingleOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewModel.Book = book;
            return View(ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != model.Book.BookId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                *//*ViewModel.Book = model.Book;
                return View(ViewModel);*//*
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                return Json(new { errors });
            }
            var dbModel = _db.Books.Include(b => b.Category).Where(b => b.BookId == id).FirstOrDefault();

            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                var RootDirectory = _hostingEnvironment.WebRootPath;
                var extension = Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine(DataContext.ImageDirectory, model.Book.BookId + extension);
                using (var fileStream = new FileStream(Path.Combine(RootDirectory, filePath), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                dbModel.ImagePath = @"/" + filePath;
            }
            dbModel.AuthorId = model.Book.AuthorId;
            dbModel.CategoryId = model.Book.CategoryId;
            dbModel.Discount = model.Book.Discount;
            dbModel.Price = model.Book.Price;
            dbModel.Stock = model.Book.Stock;
            dbModel.Title = model.Book.Title;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }*/


    }
}
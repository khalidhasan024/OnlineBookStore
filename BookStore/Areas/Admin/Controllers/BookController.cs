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

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;


        public BookViewModel ViewModel { get; set; }
        public BookController(ApplicationDbContext db, HostingEnvironment host)
        {
            _db = db;
            _hostingEnvironment = host;
            ViewModel = new BookViewModel()
            {
                Book = new Models.Book(),
                Authors = _db.Authors.ToList(),
                BookCategories = _db.BookCategories.ToList(),
                Tags = _db.Tags.ToList()
            };
        }
        public async Task<IActionResult> Index()
        {
            var books = _db.Books.Include(b => b.Category).Include(b => b.Author).Include(b => b.BookTags);
            return View(await books.ToListAsync());
        }

        public IActionResult Create()
        {
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewModel.Book = model.Book;
                return View(ViewModel);
            }

            _db.Books.Add(model.Book);
            await _db.SaveChangesAsync();

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

                model.Book.ImagePath = @"/images/books/" + model.Book.BookId + extension;

            }
            else
            {
                _db.Attach(model.Book);
                model.Book.ImagePath = @"/" + Path.Combine(DataContext.ImageDirectory + DataContext.DefaultImage);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //Give this a name other than view model for example BookViewModel
            BookViewModel model = new BookViewModel();

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

            model.Book = book;
            model.Authors = _db.Authors.ToList();
            model.BookCategories = _db.BookCategories.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model, int? id)
        {
            if (id == null || id != model.Book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var dbModel = _db.Books.Include(b => b.Category).Where(b => b.BookId == id).FirstOrDefault();

                var files = HttpContext.Request.Form.Files;
                if (files.Any())
                {
                    var RootDirectory = _hostingEnvironment.WebRootPath;
                    var img = Path.Combine(RootDirectory, DataContext.ImageDirectory, Path.GetFileName(dbModel.ImagePath));

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

                    dbModel.ImagePath = @"/images/books/" + model.Book.BookId + extension;
                }

                dbModel.AuthorId = model.Book.AuthorId;
                dbModel.CategoryId = model.Book.CategoryId;
                dbModel.Discount = model.Book.Discount;
                dbModel.Price = model.Book.Price;
                dbModel.Stock = model.Book.Stock;
                dbModel.Title = model.Book.Title;

                //_db.Books.Update(dbModel);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(model);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var book = await _db.Books.Include(b => b.Category).Include(b => b.Author)
                        .Where(b => b.BookId == id).FirstOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var book = await _db.Books.Include(b => b.Category).Include(b => b.Author)
                        .Where(b => b.BookId == id).FirstOrDefaultAsync();
            if(book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var book = await _db.Books.SingleOrDefaultAsync(b => b.BookId == id);
            if(book == null)
            {
                return NotFound();
            }
            var RootDirectory = _hostingEnvironment.WebRootPath;
            var img = Path.Combine(RootDirectory, DataContext.ImageDirectory, Path.GetFileName(book.ImagePath));

            if (System.IO.File.Exists(img))
            {
                System.IO.File.Delete(img);
            }
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
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
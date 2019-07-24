using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TagController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Tags.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if(ModelState.IsValid)
            {
                _db.Tags.Add(tag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var tag = await _db.Tags.FindAsync(id);
            if(tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Tag tag)
        {
            if(id != tag.Id)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _db.Tags.Update(tag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var tag = await _db.Tags.FindAsync(id);
            if(tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var tag = await _db.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, Tag tag)
        {
            if(id != tag.Id)
            {
                return NotFound();
            }
            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
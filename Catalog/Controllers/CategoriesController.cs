﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catalog.Data;
using Catalog.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Catalog.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        //[Authorize(Roles = "admin")]
        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }


        //[Authorize(Roles = "admin")]
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Categories";
            _context.Add(logg);
            await _context.SaveChangesAsync();

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [Authorize(Roles = "admin")]
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();

                var logg = new Catalog.Entities.Logg();
                logg.Operation = "insert";
                logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
                logg.Date = DateTime.Now.ToString();
                logg.Table = "Categories";
                _context.Add(logg);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [Authorize(Roles = "admin")]
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Categories";
            _context.Add(logg);
            await _context.SaveChangesAsync();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();

                    var logg = new Catalog.Entities.Logg();
                    logg.Operation = "uodate";
                    logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
                    logg.Date = DateTime.Now.ToString();
                    logg.Table = "Categories";
                    _context.Add(logg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        [Authorize(Roles = "admin")]
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Categories";
            _context.Add(logg);
            await _context.SaveChangesAsync();

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                var logg = new Catalog.Entities.Logg();
                logg.Operation = "delete";
                logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
                logg.Date = DateTime.Now.ToString();
                logg.Table = "Categories";
                _context.Add(logg);
                await _context.SaveChangesAsync();

            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "admin")]
        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

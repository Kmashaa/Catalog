﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catalog.Data;
using Catalog.Entities;

namespace Catalog.Controllers
{
    public class CreditsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreditsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Credits
        public async Task<IActionResult> Index()
        {
              return _context.Credits != null ? 
                          View(await _context.Credits.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Credits'  is null.");
        }

        // GET: Credits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Credits == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (credit == null)
            {
                return NotFound();
            }

            return View(credit);
        }

        // GET: Credits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Credits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Percents,CategoryId")] Credit credit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(credit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(credit);
        }

        // GET: Credits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Credits == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits.FindAsync(id);
            if (credit == null)
            {
                return NotFound();
            }
            return View(credit);
        }

        // POST: Credits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Percents,CategoryId")] Credit credit)
        {
            if (id != credit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(credit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditExists(credit.Id))
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
            return View(credit);
        }

        // GET: Credits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Credits == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (credit == null)
            {
                return NotFound();
            }

            return View(credit);
        }

        // POST: Credits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Credits == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Credits'  is null.");
            }
            var credit = await _context.Credits.FindAsync(id);
            if (credit != null)
            {
                _context.Credits.Remove(credit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditExists(int id)
        {
          return (_context.Credits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
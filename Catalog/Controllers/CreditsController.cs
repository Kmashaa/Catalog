﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catalog.Data;
using Catalog.Entities;
using Catalog.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Catalog.Controllers
{
    public class CreditsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreditsController(ApplicationDbContext context)
        {
            _context = context;
        }


        private static int? gr=0;

        // GET: Credits
        public async Task<IActionResult> Index(int? group, int? minPercents, int? maxPercents, int? minMonth, int? maxMonth)
        {
            if (group != null)
            {
                gr = group;
            }

            //var minimum = minPercents;
            //var maximum = maxPercents;
            //var categ= _context.Categories.ToList();
            ViewBag.categories = _context.Categories.ToList();
            var creditsFiltered = _context.Credits.Where(d => !group.HasValue || d.CategoryId == gr.Value);
            if (maxPercents != null)
            {
                creditsFiltered = creditsFiltered.Where(d => d.Percents <= maxPercents);
            }

            if (minPercents != null)
            {
                creditsFiltered = creditsFiltered.Where(d => d.Percents >= minPercents);
            }

            if (maxMonth != null)
            {
                creditsFiltered = creditsFiltered.Where(d => d.Months <= maxMonth);
            }

            if (minMonth != null)
            {
                creditsFiltered = creditsFiltered.Where(d => d.Months >= minMonth);
            }



            //var param = new Microsoft.Data.SqlClient.SqlParameter
            //{
            //    ParameterName = "@userName",
            //    SqlDbType = System.Data.SqlDbType.VarChar,
            //    Direction = System.Data.ParameterDirection.Output,
            //    Size = 50
            //};
            //_context.Database.ExecuteSqlRaw("GetCredits @userName OUT");
            //Console.WriteLine(param.Value);
            //var user = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList();

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Credits";
            _context.Add(logg);
            await _context.SaveChangesAsync();

            return _context.Credits != null ?
                View(creditsFiltered) :
                          //View(await _context.Credits.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Credits'  is null.");
        }

        // GET: Credits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.categories = _context.Categories.ToList();

            if (id == null || _context.Credits == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits
                .FirstOrDefaultAsync(m => m.Id == id);

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Credits";
            _context.Add(logg);
            await _context.SaveChangesAsync();

            if (credit == null)
            {
                return NotFound();
            }

            return View(credit);
        }

        // GET: Credits/Create
        public IActionResult Create()
        {
            ViewBag.categories = _context.Categories.ToList();
            return View();
        }

        // POST: Credits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Percents,Months,CategoryId,GeneralNote,SpecialNote")] Credit credit)
        {
            ViewBag.categories = _context.Categories.ToList();

            if (ModelState.IsValid)
            {
                _context.Add(credit);
                await _context.SaveChangesAsync();

                var logg = new Catalog.Entities.Logg();
                logg.Operation = "insert";
                logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
                logg.Date = DateTime.Now.ToString();
                logg.Table = "Credits";
                _context.Add(logg);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(credit);
        }


        public async Task<IActionResult> Calculator(int? id)
        {
            var credit = await _context.Credits.FindAsync(id);

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Credits";
            _context.Add(logg);
            await _context.SaveChangesAsync();

            return View(credit);
        }

        public async Task<IActionResult> Calc(int? id, int? percents, int? months, int? sum)
        {
            double percentsPerMonth = (double)((double)percents / (100 * 12));
            var Pen = sum * (percentsPerMonth / (1 - Math.Pow(1 + percentsPerMonth, (int)months * (-1))));
            var totalOverpayment = Pen * months;
            var total = totalOverpayment + sum;
            List<double> tmp = new List<double>() { Math.Round((double)Pen,2),  Math.Round((double)totalOverpayment,2), Math.Round((double)total, 2) };
            return View(tmp);
        }


        // GET: Credits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.categories = _context.Categories.ToList();

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Credits";
            _context.Add(logg);
            await _context.SaveChangesAsync();

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
            ViewBag.categories = _context.Categories.ToList();

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

                    var logg = new Catalog.Entities.Logg();
                    logg.Operation = "update";
                    logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
                    logg.Date = DateTime.Now.ToString();
                    logg.Table = "Credits";
                    _context.Add(logg);
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
            ViewBag.categories = _context.Categories.ToList();
            if (id == null || _context.Credits == null)
            {
                return NotFound();
            }

            var credit = await _context.Credits
                .FirstOrDefaultAsync(m => m.Id == id);

            var logg = new Catalog.Entities.Logg();
            logg.Operation = "select";
            logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
            logg.Date = DateTime.Now.ToString();
            logg.Table = "Credits";
            _context.Add(logg);
            await _context.SaveChangesAsync();

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
            ViewBag.categories = _context.Categories.ToList();

            if (_context.Credits == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Credits'  is null.");
            }
            var credit = await _context.Credits.FindAsync(id);
            if (credit != null)
            {
                _context.Credits.Remove(credit);

                var logg = new Catalog.Entities.Logg();
                logg.Operation = "delete";
                logg.UserId = _context.Users.Where(d => d.UserName == User.Identity.Name).ToList()[0].Id;
                logg.Date = DateTime.Now.ToString();
                logg.Table = "Credits";
                _context.Add(logg);
                await _context.SaveChangesAsync();
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

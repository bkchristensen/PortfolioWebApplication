using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortfolioWebApplication.Models;

namespace PortfolioWebApplication.Controllers
{
    public class ScriptsController : Controller
    {
        private readonly ScriptContext _context;

        public ScriptsController(ScriptContext context)
        {
            _context = context;
        }

        // GET: Scripts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScriptItems.ToListAsync());
        }

        // GET: Scripts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var script = await _context.ScriptItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (script == null)
            {
                return NotFound();
            }

            return View(script);
        }

        // GET: Scripts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scripts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Class,CodeReferences,Code")] Script script)
        {
            if (ModelState.IsValid)
            {
                _context.Add(script);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(script);
        }

        // GET: Scripts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var script = await _context.ScriptItems.FindAsync(id);
            if (script == null)
            {
                return NotFound();
            }
            return View(script);
        }

        // POST: Scripts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Class,CodeReferences,Code")] Script script)
        {
            if (id != script.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(script);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScriptExists(script.Id))
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
            return View(script);
        }

        // GET: Scripts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var script = await _context.ScriptItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (script == null)
            {
                return NotFound();
            }

            return View(script);
        }

        // POST: Scripts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var script = await _context.ScriptItems.FindAsync(id);
            _context.ScriptItems.Remove(script);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScriptExists(int id)
        {
            return _context.ScriptItems.Any(e => e.Id == id);
        }
    }
}

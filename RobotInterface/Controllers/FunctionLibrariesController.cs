using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RobotInterface.Data;
using RobotInterface.Models;

namespace RobotInterface.Controllers
{
    public class FunctionLibrariesController : Controller
    {
        private readonly RobotInterfaceContext _context;

        public FunctionLibrariesController(RobotInterfaceContext context)
        {
            _context = context;
        }

        // GET: FunctionLibraries
        public async Task<IActionResult> Index()
        {
            var robotInterfaceContext = _context.FunctionLibrary.Include(f => f.Function).Include(f => f.Library);
            return View(await robotInterfaceContext.ToListAsync());
        }

        // GET: FunctionLibraries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionLibrary = await _context.FunctionLibrary
                .Include(f => f.Function)
                .Include(f => f.Library)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionLibrary == null)
            {
                return NotFound();
            }

            return View(functionLibrary);
        }

        // GET: FunctionLibraries/Create
        public IActionResult Create()
        {
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id");
            ViewData["LibraryId"] = new SelectList(_context.Set<Library>(), "Id", "Id");
            return View();
        }

        // POST: FunctionLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FunctionId,LibraryId")] FunctionLibrary functionLibrary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functionLibrary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id", functionLibrary.FunctionId);
            ViewData["LibraryId"] = new SelectList(_context.Set<Library>(), "Id", "Id", functionLibrary.LibraryId);
            return View(functionLibrary);
        }

        // GET: FunctionLibraries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionLibrary = await _context.FunctionLibrary.FindAsync(id);
            if (functionLibrary == null)
            {
                return NotFound();
            }
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id", functionLibrary.FunctionId);
            ViewData["LibraryId"] = new SelectList(_context.Set<Library>(), "Id", "Id", functionLibrary.LibraryId);
            return View(functionLibrary);
        }

        // POST: FunctionLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FunctionId,LibraryId")] FunctionLibrary functionLibrary)
        {
            if (id != functionLibrary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functionLibrary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionLibraryExists(functionLibrary.Id))
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
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id", functionLibrary.FunctionId);
            ViewData["LibraryId"] = new SelectList(_context.Set<Library>(), "Id", "Id", functionLibrary.LibraryId);
            return View(functionLibrary);
        }

        // GET: FunctionLibraries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionLibrary = await _context.FunctionLibrary
                .Include(f => f.Function)
                .Include(f => f.Library)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionLibrary == null)
            {
                return NotFound();
            }

            return View(functionLibrary);
        }

        // POST: FunctionLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functionLibrary = await _context.FunctionLibrary.FindAsync(id);
            if (functionLibrary != null)
            {
                _context.FunctionLibrary.Remove(functionLibrary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionLibraryExists(int id)
        {
            return _context.FunctionLibrary.Any(e => e.Id == id);
        }
    }
}

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
    public class FunctionCommandsController : Controller
    {
        private readonly RobotInterfaceContext _context;

        public FunctionCommandsController(RobotInterfaceContext context)
        {
            _context = context;
        }

        // GET: FunctionCommands
        public async Task<IActionResult> Index()
        {
            var robotInterfaceContext = _context.FunctionCommand.Include(f => f.Function);
            return View(await robotInterfaceContext.ToListAsync());
        }

        // GET: FunctionCommands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionCommand = await _context.FunctionCommand
                .Include(f => f.Function)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionCommand == null)
            {
                return NotFound();
            }

            return View(functionCommand);
        }

        // GET: FunctionCommands/Create
        public IActionResult Create()
        {
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id");
            return View();
        }

        // POST: FunctionCommands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FunctionId,CommadnId")] FunctionCommand functionCommand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functionCommand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id", functionCommand.FunctionId);
            return View(functionCommand);
        }

        // GET: FunctionCommands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionCommand = await _context.FunctionCommand.FindAsync(id);
            if (functionCommand == null)
            {
                return NotFound();
            }
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id", functionCommand.FunctionId);
            return View(functionCommand);
        }

        // POST: FunctionCommands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FunctionId,CommadnId")] FunctionCommand functionCommand)
        {
            if (id != functionCommand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functionCommand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionCommandExists(functionCommand.Id))
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
            ViewData["FunctionId"] = new SelectList(_context.Function, "Id", "Id", functionCommand.FunctionId);
            return View(functionCommand);
        }

        // GET: FunctionCommands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionCommand = await _context.FunctionCommand
                .Include(f => f.Function)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functionCommand == null)
            {
                return NotFound();
            }

            return View(functionCommand);
        }

        // POST: FunctionCommands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functionCommand = await _context.FunctionCommand.FindAsync(id);
            if (functionCommand != null)
            {
                _context.FunctionCommand.Remove(functionCommand);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionCommandExists(int id)
        {
            return _context.FunctionCommand.Any(e => e.Id == id);
        }
    }
}

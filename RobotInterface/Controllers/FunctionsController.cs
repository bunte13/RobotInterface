using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RobotInterface.Data;
using RobotInterface.Models;
using RobotInterface.ViewModels;

namespace RobotInterface.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly RobotInterfaceContext _context;

        public FunctionsController(RobotInterfaceContext context)
        {
            _context = context;
        }

        // GET: Functions
        public async Task<IActionResult> Index()
        {
            var robotInterfaceContext = _context.Function.Include(f => f.Category);
            return View(await robotInterfaceContext.ToListAsync());
        }

        // GET: Functions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.Function
                .Include(f => f.Category)
                .Include(f => f.FunctionCommands)
                .ThenInclude(f=>f.Command)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // GET: Functions/Create
        public IActionResult Create()
        {
            var Libraries= _context.Library.ToList();
            var Commands = _context.Command.ToList();
            var categories = _context.Category.ToList();

            var viewModel = new FunctionCommandLibrariesEditCreate
            {
                CommandsList = new MultiSelectList(Commands, "Id", "CommandName"),
                LibrariesList = new MultiSelectList(Libraries, "Id", "LibraryName"),
                CategoryList = new SelectList(categories,"Id","CategoryName")
            };

            return View(viewModel);
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FunctionCommandLibrariesEditCreate viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Function.CategoryId = viewModel.SelectedCategoryId;
                _context.Add(viewModel.Function);
                await _context.SaveChangesAsync();

                foreach (int item in viewModel.SelectedCommands)
                {
                    _context.FunctionCommand.Add(new FunctionCommand { CommandId = item, FunctionId = viewModel.Function.Id });
                }
                foreach (int item in viewModel.SelectedLibraries)
                {
                    _context.FunctionLibrary.Add(new FunctionLibrary { LibraryId = item, FunctionId = viewModel.Function.Id });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var commands = _context.Command.ToList();
            viewModel.CommandsList = new MultiSelectList(commands, "Id", "CommandName");
            var libraries = _context.Library.ToList();
            viewModel.LibrariesList = new MultiSelectList(libraries, "Id", "LibraryName");
            var categories = _context.Category.ToList();
            viewModel.CategoryList = new SelectList(categories, "Id", "CategoryName");

            return View(viewModel);
        
            
        }
        // GET: Functions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.Function
                .Include(f => f.FunctionLibraries)
                .Include(f => f.FunctionCommands)
                .FirstOrDefaultAsync(f => f.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            var viewModel = new FunctionCommandLibrariesEditCreate
            {
                Function = function,
                CommandsList = new MultiSelectList(_context.Command, "Id", "CommandName"),
                LibrariesList = new MultiSelectList(_context.Library, "Id", "LibraryName"),
                CategoryList = new SelectList(_context.Category, "Id", "CategoryName"),
                SelectedLibraries = function.FunctionLibraries.Select(fl => fl.LibraryId).ToList(),
                SelectedCommands = function.FunctionCommands.Select(fc => fc.CommandId).ToList()
            };

            return View(viewModel);
        }



        // POST: Functions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FunctionCommandLibrariesEditCreate viewModel)
        {
            if (id != viewModel.Function.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Function.CategoryId = viewModel.SelectedCategoryId;

                    _context.Update(viewModel.Function);
                    await _context.SaveChangesAsync();

                    // Update FunctionCommands
                    var oldCommands = _context.FunctionCommand.Where(bg => bg.FunctionId == id);
                    _context.FunctionCommand.RemoveRange(oldCommands);
                    await _context.SaveChangesAsync();

                    foreach (int comId in viewModel.SelectedCommands)
                    {
                        _context.FunctionCommand.Add(new FunctionCommand { CommandId = comId, FunctionId = id });
                    }

                    // Update FunctionLibraries
                    var oldLibraries = _context.FunctionLibrary.Where(bg => bg.FunctionId == id);
                    _context.FunctionLibrary.RemoveRange(oldLibraries);
                    await _context.SaveChangesAsync();

                    foreach (int libId in viewModel.SelectedLibraries)
                    {
                        _context.FunctionLibrary.Add(new FunctionLibrary { LibraryId = libId, FunctionId = id });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                    ModelState.AddModelError("", "An error occurred while updating the item. Please try again later.");
                }

                return RedirectToAction(nameof(Index));
            }

            // Repopulate view model if ModelState is not valid
            var categories = _context.Category.ToList();
            var libraries = _context.Library.ToList();
            var commands = _context.Command.ToList();
            viewModel.CommandsList = new MultiSelectList(commands, "Id", "CommandName");
            viewModel.LibrariesList = new MultiSelectList(libraries, "Id", "LibraryName");
            viewModel.CategoryList = new SelectList(categories, "Id", "CategoryName");

            return View(viewModel);
        }


        // GET: Functions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var function = await _context.Function
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (function == null)
            {
                return NotFound();
            }

            return View(function);
        }

        // POST: Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var function = await _context.Function.FindAsync(id);
            if (function != null)
            {
                _context.Function.Remove(function);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionExists(int id)
        {
            return _context.Function.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using RobotInterface.Data;
using RobotInterface.Models;
using RobotInterface.ViewModels;

namespace RobotInterface.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly RobotInterfaceContext _context;

        public FileUploadController(RobotInterfaceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(FileUploadViewModel model)
        {
            if (model.ExcelFile != null && model.ExcelFile.Length > 0)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        await model.ExcelFile.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            var workbook = package.Workbook;
                            if (workbook == null)
                            {
                                ModelState.AddModelError("", "Cannot access the workbook. Ensure the Excel file is in the correct format and not corrupted.");
                                return View();
                            }

                            // Process Categories Sheet
                            var categoryWorksheet = workbook.Worksheets["Categories"];
                            if (categoryWorksheet == null)
                            {
                                ModelState.AddModelError("", "The 'Categories' worksheet was not found in the Excel file.");
                                return View();
                            }
                            var categories = new List<Category>();
                            for (int row = 2; row <= categoryWorksheet.Dimension.Rows; row++)
                            {
                                var categoryName = categoryWorksheet.Cells[row, 1].Text.Trim();
                                var description = categoryWorksheet.Cells[row, 2].Text.Trim();

                                if (string.IsNullOrWhiteSpace(categoryName) ||
                                    string.IsNullOrWhiteSpace(description))
                                {
                                    // Skip empty or invalid rows
                                    continue;
                                }

                                var category = new Category
                                {
                                    CategoryName = categoryName,
                                    Description = description
                                };

                                categories.Add(category);
                            }
                            await _context.Category.AddRangeAsync(categories);
                            await _context.SaveChangesAsync();

                            // Process Functions Sheet
                            var functionWorksheet = workbook.Worksheets["Functions"];
                            if (functionWorksheet == null)
                            {
                                ModelState.AddModelError("", "The 'Functions' worksheet was not found in the Excel file.");
                                return View();
                            }
                            var functions = new List<Function>();
                            for (int row = 2; row <= functionWorksheet.Dimension.Rows; row++)
                            {
                                var functionName = functionWorksheet.Cells[row, 1].Text.Trim();
                                var categoryIdText = functionWorksheet.Cells[row, 2].Text.Trim();

                                if (string.IsNullOrWhiteSpace(functionName) ||
                                    string.IsNullOrWhiteSpace(categoryIdText))
                                {
                                    // Skip empty or invalid rows
                                    continue;
                                }

                                if (int.TryParse(categoryIdText, out int categoryId))
                                {
                                    if (!_context.Category.Any(c => c.Id == categoryId))
                                    {
                                        ModelState.AddModelError("", $"Category with Id {categoryId} does not exist.");
                                        return View();
                                    }

                                    var function = new Function
                                    {
                                        FunctionName = functionName,
                                        CategoryId = categoryId
                                    };

                                    functions.Add(function);
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Invalid data format at row {row} in 'Functions' worksheet.");
                                    return View();
                                }
                            }
                            await _context.Function.AddRangeAsync(functions);
                            await _context.SaveChangesAsync();

                            // Process Commands Sheet
                            var commandWorksheet = workbook.Worksheets["Commands"];
                            if (commandWorksheet == null)
                            {
                                ModelState.AddModelError("", "The 'Commands' worksheet was not found in the Excel file.");
                                return View();
                            }
                            var commands = new List<Command>();
                            for (int row = 2; row <= commandWorksheet.Dimension.Rows; row++)
                            {
                                var commandName = commandWorksheet.Cells[row, 1].Text.Trim();
                                

                                if (string.IsNullOrWhiteSpace(commandName))
                                    
                                {
                                    // Skip empty or invalid rows
                                    continue;
                                }

                                var command = new Command
                                {
                                    CommandName = commandName,
                                    
                                };

                                commands.Add(command);
                            }
                            await _context.Command.AddRangeAsync(commands);
                            await _context.SaveChangesAsync();
                            // Process Libraries Sheet
                            var libraryWorksheet = workbook.Worksheets["Libraries"];
                            if (libraryWorksheet == null)
                            {
                                ModelState.AddModelError("", "The 'Libraries' worksheet was not found in the Excel file.");
                                return View();
                            }
                            var libraries = new List<Library>();
                            for (int row = 2; row <= libraryWorksheet.Dimension.Rows; row++)
                            {
                                var libraryName = libraryWorksheet.Cells[row, 1].Text.Trim();
                                var description = libraryWorksheet.Cells[row, 2].Text.Trim();

                                if (string.IsNullOrWhiteSpace(libraryName) ||
                                    string.IsNullOrWhiteSpace(description))
                                {
                                    // Skip empty or invalid rows
                                    continue;
                                }

                                var library = new Library
                                {
                                    LibraryName = libraryName,
                                    Description = description
                                };

                                libraries.Add(library);
                            }
                            await _context.Library.AddRangeAsync(libraries);
                            await _context.SaveChangesAsync();

                            // Process FunctionLibraries Sheet
                            var functionLibraryWorksheet = workbook.Worksheets["FunctionLibraries"];
                            if (functionLibraryWorksheet == null)
                            {
                                ModelState.AddModelError("", "The 'FunctionLibraries' worksheet was not found in the Excel file.");
                                return View();
                            }
                            var functionLibraries = new List<FunctionLibrary>();
                            for (int row = 2; row <= functionLibraryWorksheet.Dimension.Rows; row++)
                            {
                                var functionIdText = functionLibraryWorksheet.Cells[row, 1].Text.Trim();
                                var libraryIdText = functionLibraryWorksheet.Cells[row, 2].Text.Trim();

                                if (string.IsNullOrWhiteSpace(functionIdText) ||
                                    string.IsNullOrWhiteSpace(libraryIdText))
                                {
                                    // Skip empty or invalid rows
                                    continue;
                                }

                                if (int.TryParse(functionIdText, out int functionId) &&
                                    int.TryParse(libraryIdText, out int libraryId))
                                {
                                    if (!_context.Function.Any(f => f.Id == functionId))
                                    {
                                        ModelState.AddModelError("", $"Function with Id {functionId} does not exist.");
                                        return View();
                                    }
                                    if (!_context.Library.Any(l => l.Id == libraryId))
                                    {
                                        ModelState.AddModelError("", $"Library with Id {libraryId} does not exist.");
                                        return View();
                                    }

                                    var functionLibrary = new FunctionLibrary
                                    {
                                        FunctionId = functionId,
                                        LibraryId = libraryId
                                    };

                                    functionLibraries.Add(functionLibrary);
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Invalid data format at row {row} in 'FunctionLibraries' worksheet.");
                                    return View();
                                }
                            }
                            await _context.FunctionLibrary.AddRangeAsync(functionLibraries);
                            await _context.SaveChangesAsync();

                            // Process FunctionCommands Sheet
                            var functionCommandWorksheet = workbook.Worksheets["FunctionCommands"];
                            if (functionCommandWorksheet == null)
                            {
                                ModelState.AddModelError("", "The 'FunctionCommands' worksheet was not found in the Excel file.");
                                return View();
                            }
                            var functionCommands = new List<FunctionCommand>();
                            for (int row = 2; row <= functionCommandWorksheet.Dimension.Rows; row++)
                            {
                                var functionIdText = functionCommandWorksheet.Cells[row, 1].Text.Trim();
                                var commandIdText = functionCommandWorksheet.Cells[row, 2].Text.Trim();

                                if (string.IsNullOrWhiteSpace(functionIdText) ||
                                    string.IsNullOrWhiteSpace(commandIdText))
                                {
                                    // Skip empty or invalid rows
                                    continue;
                                }

                                if (int.TryParse(functionIdText, out int functionId) &&
                                    int.TryParse(commandIdText, out int commandId))
                                {
                                    if (!_context.Function.Any(f => f.Id == functionId))
                                    {
                                        ModelState.AddModelError("", $"Function with Id {functionId} does not exist.");
                                        return View();
                                    }
                                    if (!_context.Command.Any(c => c.Id == commandId))
                                    {
                                        ModelState.AddModelError("", $"Command with Id {commandId} does not exist.");
                                        return View();
                                    }

                                    var functionCommand = new FunctionCommand
                                    {
                                        FunctionId = functionId,
                                        CommandId = commandId
                                    };

                                    functionCommands.Add(functionCommand);
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Invalid data format at row {row} in 'FunctionCommands' worksheet.");
                                    return View();
                                }
                            }
                            await _context.FunctionCommand.AddRangeAsync(functionCommands);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Extract detailed exception message
                    var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", $"An error occurred while processing the Excel file: {message}");
                    return View();
                }

                return RedirectToAction("Index", "Functions");
            }

            return View();
        }
    }
}

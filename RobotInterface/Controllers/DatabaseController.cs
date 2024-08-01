using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobotInterface.Data;
using System.Threading.Tasks;

namespace RobotInterface.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly RobotInterfaceContext _context;

        public DatabaseController(RobotInterfaceContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetDatabase()
        {
            // Deleting all records
            _context.FunctionCommand.RemoveRange(_context.FunctionCommand);
            _context.FunctionLibrary.RemoveRange(_context.FunctionLibrary);
            _context.Command.RemoveRange(_context.Command);
            _context.Library.RemoveRange(_context.Library);
            _context.Function.RemoveRange(_context.Function);
            _context.Category.RemoveRange(_context.Category);

            await _context.SaveChangesAsync();

            // Resetting auto-increment values
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Category', RESEED, 0);");
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Function', RESEED, 0);");
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Library', RESEED, 0);");
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Command', RESEED, 0);");
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('FunctionCommand', RESEED, 0);");
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('FunctionLibrary', RESEED, 0);");

            return RedirectToAction("Index", "Home");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RobotInterface.Models;

namespace RobotInterface.Data
{
    public class RobotInterfaceContext : DbContext
    {
        public RobotInterfaceContext(DbContextOptions<RobotInterfaceContext> options)
            : base(options)
        {
        }

        public DbSet<Function> Function { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Command> Command { get; set; } = default!;
        public DbSet<FunctionCommand> FunctionCommand { get; set; } = default!;
        public DbSet<FunctionLibrary> FunctionLibrary { get; set; } = default!;
        public DbSet<Library> Library { get; set; } = default!;

        // Method to reset auto-increment values
        
    }
}

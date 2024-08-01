using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobotInterface.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Libraries",
                table: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Libraries",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

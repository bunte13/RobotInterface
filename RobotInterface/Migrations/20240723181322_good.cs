using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobotInterface.Migrations
{
    /// <inheritdoc />
    public partial class good : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionCommand_Command_CommandId",
                table: "FunctionCommand");

            migrationBuilder.DropColumn(
                name: "CommadnId",
                table: "FunctionCommand");

            migrationBuilder.AlterColumn<int>(
                name: "CommandId",
                table: "FunctionCommand",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionCommand_Command_CommandId",
                table: "FunctionCommand",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionCommand_Command_CommandId",
                table: "FunctionCommand");

            migrationBuilder.AlterColumn<int>(
                name: "CommandId",
                table: "FunctionCommand",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CommadnId",
                table: "FunctionCommand",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionCommand_Command_CommandId",
                table: "FunctionCommand",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "Id");
        }
    }
}

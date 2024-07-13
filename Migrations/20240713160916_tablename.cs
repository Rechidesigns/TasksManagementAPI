using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class tablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_taskManagers",
                table: "taskManagers");

            migrationBuilder.RenameTable(
                name: "taskManagers",
                newName: "TaskManagers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskManagers",
                table: "TaskManagers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskManagers",
                table: "TaskManagers");

            migrationBuilder.RenameTable(
                name: "TaskManagers",
                newName: "taskManagers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_taskManagers",
                table: "taskManagers",
                column: "Id");
        }
    }
}

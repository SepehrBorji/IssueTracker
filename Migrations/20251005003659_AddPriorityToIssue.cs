using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.ConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityToIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriorityLevel",
                table: "Issues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriorityLevel",
                table: "Issues");
        }
    }
}

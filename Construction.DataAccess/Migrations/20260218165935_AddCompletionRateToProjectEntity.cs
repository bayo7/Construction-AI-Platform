using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletionRateToProjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompleetionRate",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleetionRate",
                table: "Projects");
        }
    }
}

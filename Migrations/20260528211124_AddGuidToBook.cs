using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace marsian_library.Migrations
{
    /// <inheritdoc />
    public partial class AddGuidToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                schema: "SYSTEM",
                table: "Books",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "SYSTEM",
                table: "Books");
        }
    }
}

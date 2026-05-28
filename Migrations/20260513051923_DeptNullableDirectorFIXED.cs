using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace marsian_library.Migrations
{
    /// <inheritdoc />
    public partial class DeptNullableDirectorFIXED : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DirectorId",
                schema: "SYSTEM",
                table: "Depts",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DirectorId",
                schema: "SYSTEM",
                table: "Depts",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);
        }
    }
}

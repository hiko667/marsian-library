using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace marsian_library.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReaderId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "EmpId",
                unique: true,
                filter: "\"EmpId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReaderId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "ReaderId",
                unique: true,
                filter: "\"ReaderId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Emps_EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "EmpId",
                principalSchema: "SYSTEM",
                principalTable: "Emps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Readers_ReaderId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "ReaderId",
                principalSchema: "SYSTEM",
                principalTable: "Readers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Emps_EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Readers_ReaderId",
                schema: "SYSTEM",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReaderId",
                schema: "SYSTEM",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReaderId",
                schema: "SYSTEM",
                table: "AspNetUsers");
        }
    }
}

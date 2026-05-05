using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace marsian_library.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryCoreStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    City = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Building = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    Apartment = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true),
                    ZipCode = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FirstName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    ChildrenFriendly = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AddressId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "SYSTEM",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Title = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: false),
                    Isbn = table.Column<string>(type: "NVARCHAR2(13)", maxLength: 13, nullable: false),
                    PublisherId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalSchema: "SYSTEM",
                        principalTable: "Publishers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                schema: "SYSTEM",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    BooksId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalSchema: "SYSTEM",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksId",
                        column: x => x.BooksId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                schema: "SYSTEM",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AuthorId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "SYSTEM",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                schema: "SYSTEM",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    GenresId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BooksId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BookGenre_Books_BooksId",
                        column: x => x.BooksId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalSchema: "SYSTEM",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenres",
                schema: "SYSTEM",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    GenreId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "SYSTEM",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookLanguage",
                schema: "SYSTEM",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LanguagesId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLanguage", x => new { x.BooksId, x.LanguagesId });
                    table.ForeignKey(
                        name: "FK_BookLanguage_Books_BooksId",
                        column: x => x.BooksId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLanguage_Languages_LanguagesId",
                        column: x => x.LanguagesId,
                        principalSchema: "SYSTEM",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookLanguages",
                schema: "SYSTEM",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LanguageId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLanguages", x => new { x.BookId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_BookLanguages_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "SYSTEM",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CopyId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ReaderId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TimesExtended = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrows_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalSchema: "SYSTEM",
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Copies",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    BookId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DeptId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    StateId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Copies_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "SYSTEM",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Copies_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "SYSTEM",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Depts",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AddressId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DirectorId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "SYSTEM",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emps",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AddressId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DeptId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    JobId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emps_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "SYSTEM",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emps_Depts_DeptId",
                        column: x => x.DeptId,
                        principalSchema: "SYSTEM",
                        principalTable: "Depts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emps_Jobs_JobId",
                        column: x => x.JobId,
                        principalSchema: "SYSTEM",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                schema: "SYSTEM",
                table: "AuthorBook",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                schema: "SYSTEM",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenresId",
                schema: "SYSTEM",
                table: "BookGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                schema: "SYSTEM",
                table: "BookGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLanguage_LanguagesId",
                schema: "SYSTEM",
                table: "BookLanguage",
                column: "LanguagesId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLanguages_LanguageId",
                schema: "SYSTEM",
                table: "BookLanguages",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn",
                schema: "SYSTEM",
                table: "Books",
                column: "Isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                schema: "SYSTEM",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_CopyId",
                schema: "SYSTEM",
                table: "Borrows",
                column: "CopyId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_ReaderId",
                schema: "SYSTEM",
                table: "Borrows",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Copies_BookId",
                schema: "SYSTEM",
                table: "Copies",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Copies_DeptId",
                schema: "SYSTEM",
                table: "Copies",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_Copies_StateId",
                schema: "SYSTEM",
                table: "Copies",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Depts_AddressId",
                schema: "SYSTEM",
                table: "Depts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Depts_DirectorId",
                schema: "SYSTEM",
                table: "Depts",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Emps_AddressId",
                schema: "SYSTEM",
                table: "Emps",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Emps_DeptId",
                schema: "SYSTEM",
                table: "Emps",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_Emps_JobId",
                schema: "SYSTEM",
                table: "Emps",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_AddressId",
                schema: "SYSTEM",
                table: "Readers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Copies_CopyId",
                schema: "SYSTEM",
                table: "Borrows",
                column: "CopyId",
                principalSchema: "SYSTEM",
                principalTable: "Copies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Copies_Depts_DeptId",
                schema: "SYSTEM",
                table: "Copies",
                column: "DeptId",
                principalSchema: "SYSTEM",
                principalTable: "Depts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Depts_Emps_DirectorId",
                schema: "SYSTEM",
                table: "Depts",
                column: "DirectorId",
                principalSchema: "SYSTEM",
                principalTable: "Emps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emps_Depts_DeptId",
                schema: "SYSTEM",
                table: "Emps");

            migrationBuilder.DropTable(
                name: "AuthorBook",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "BookAuthors",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "BookGenre",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "BookGenres",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "BookLanguage",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "BookLanguages",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Borrows",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Copies",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Readers",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "States",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Publishers",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Depts",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Emps",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "SYSTEM");
        }
    }
}

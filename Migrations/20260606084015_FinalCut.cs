using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace marsian_library.Migrations
{
    /// <inheritdoc />
    public partial class FinalCut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SYSTEM");

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
                name: "AspNetRoles",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
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
                    Name = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
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
                name: "AspNetRoleClaims",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RoleId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "SYSTEM",
                        principalTable: "AspNetRoles",
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
                    Guid = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
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
                name: "AspNetUserClaims",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "SYSTEM",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "SYSTEM",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RoleId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "SYSTEM",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "SYSTEM",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ReaderId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EmpId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    UserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalSchema: "SYSTEM",
                        principalTable: "Readers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "SYSTEM",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "SYSTEM",
                        principalTable: "AspNetUsers",
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
                    DirectorId = table.Column<int>(type: "NUMBER(10)", nullable: true)
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
                    DeptId = table.Column<int>(type: "NUMBER(10)", nullable: true),
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
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "SYSTEM",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "SYSTEM",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "\"NormalizedName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "SYSTEM",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "SYSTEM",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "SYSTEM",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "NormalizedEmail");

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

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "\"NormalizedUserName\" IS NOT NULL");

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
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "SYSTEM",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "SYSTEM",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "SYSTEM",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "SYSTEM",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "SYSTEM",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "SYSTEM",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Emps_EmpId",
                schema: "SYSTEM",
                table: "AspNetUsers",
                column: "EmpId",
                principalSchema: "SYSTEM",
                principalTable: "Emps",
                principalColumn: "Id");

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
                name: "FK_Depts_Emps_DirectorId",
                schema: "SYSTEM",
                table: "Depts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "SYSTEM");

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
                name: "AspNetRoles",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
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
                name: "Emps",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Depts",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "SYSTEM");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "SYSTEM");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using marsian_library.Models;
using Microsoft.EntityFrameworkCore;

namespace marsian_library.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Employee", "Reader", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedAddressesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Addresses.AnyAsync())
            {
                var addresses = new List<Address>
                {
                    new Address { City = "Forge World of Mars", Street = "Main Street", Building = "Palace 1", ZipCode = "00-000" },
                    new Address { City = "Warszawa", Street = "Marszałkowska", Building = "1", Apartment = null, ZipCode = "00-001" },
                    new Address { City = "Kraków", Street = "Floriańska", Building = "12", Apartment = "3", ZipCode = "31-002" },
                    new Address { City = "Gdańsk", Street = "Długa", Building = "45", Apartment = null, ZipCode = "80-003" },
                    new Address { City = "Poznań", Street = "Święty Marcin", Building = "78", Apartment = "12A", ZipCode = "61-004" },
                    new Address { City = "Wrocław", Street = "Rynek", Building = "25", Apartment = "7", ZipCode = "50-005" },
                    new Address { City = "Łódź", Street = "Piotrkowska", Building = "89", Apartment = null, ZipCode = "90-006" },
                    new Address { City = "Katowice", Street = "Mariacka", Building = "34", Apartment = "5", ZipCode = "40-007" }
                };
                
                await context.Addresses.AddRangeAsync(addresses);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedStatesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.States.AnyAsync())
            {
                var states = new List<State>
                {
                    new State { Name = "Available" },
                    new State { Name = "Borrowed" },
                    new State { Name = "Damaged" },
                    new State { Name = "Lost" },
                    new State { Name = "In Repair" }
                };
                
                await context.States.AddRangeAsync(states);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedJobsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Jobs.AnyAsync())
            {
                var jobs = new List<Job>
                {
                    new Job { Name = "Director" },
                    new Job { Name = "Librarian" },
                    new Job { Name = "Assistant" },
                    new Job { Name = "Technician" },
                    new Job { Name = "Administrator" }
                };
                
                await context.Jobs.AddRangeAsync(jobs);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedPublishersAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Publishers.AnyAsync())
            {
                var publishers = new List<Publisher>
                {
                    new Publisher { Name = "Wydawnictwo Literackie" },
                    new Publisher { Name = "Prószyński i S-ka" },
                    new Publisher { Name = "Znak" },
                    new Publisher { Name = "Muza SA" },
                    new Publisher { Name = "Wydawnictwo Albatros" },
                    new Publisher { Name = "Wydawnictwo Mag" },
                    new Publisher { Name = "Solaris" }
                };
                
                await context.Publishers.AddRangeAsync(publishers);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedGenresAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Genres.AnyAsync())
            {
                var genres = new List<Genre>
                {
                    new Genre { Name = "Science Fiction", ChildrenFriendly = false },
                    new Genre { Name = "Fantasy", ChildrenFriendly = true },
                    new Genre { Name = "Kryminał", ChildrenFriendly = false },
                    new Genre { Name = "Horror", ChildrenFriendly = false },
                    new Genre { Name = "Romans", ChildrenFriendly = false },
                    new Genre { Name = "Literatura piękna", ChildrenFriendly = false },
                    new Genre { Name = "Bajki", ChildrenFriendly = true },
                    new Genre { Name = "Popularnonaukowa", ChildrenFriendly = false },
                    new Genre { Name = "Thriller", ChildrenFriendly = false }
                };
                
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedLanguagesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Languages.AnyAsync())
            {
                var languages = new List<Language>
                {
                    new Language { Name = "Polski" },
                    new Language { Name = "Angielski" },
                    new Language { Name = "Hiszpański" },
                    new Language { Name = "Francuski" },
                    new Language { Name = "Niemiecki" },
                    new Language { Name = "Rosyjski" }
                };
                
                await context.Languages.AddRangeAsync(languages);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAuthorsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Authors.AnyAsync())
            {
                var authors = new List<Author>
                {
                    new Author { FirstName = "Andrzej", LastName = "Sapkowski" },
                    new Author { FirstName = "Stanisław", LastName = "Lem" },
                    new Author { FirstName = "Jacek", LastName = "Dukaj" },
                    new Author { FirstName = "Olga", LastName = "Tokarczuk" },
                    new Author { FirstName = "Remigiusz", LastName = "Mróz" },
                    new Author { FirstName = "Katarzyna", LastName = "Bonda" },
                    new Author { FirstName = "Stephen", LastName = "King" },
                    new Author { FirstName = "J.R.R.", LastName = "Tolkien" },
                    new Author { FirstName = "George R.R.", LastName = "Martin" },
                    new Author { FirstName = "Frank", LastName = "Herbert" },
                    new Author { FirstName = "Isaac", LastName = "Asimov" },
                    new Author { FirstName = "Arthur C.", LastName = "Clarke" }
                };
                
                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            string adminEmail = "belisarius.cawl@mechanicus.com";
            string adminPassword = "ArchmagosDominus123!";
            
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var address = await context.Addresses.FirstOrDefaultAsync();
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.Name == "Director");
                
                if (address == null || job == null) return;

                var adminEmp = new Emp
                {
                    FirstName = "Belisarius",
                    LastName = "Cawl",
                    AddressId = address.Id,
                    JobId = job.Id,
                };

                await context.Emps.AddAsync(adminEmp);
                await context.SaveChangesAsync();

                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    EmpId = adminEmp.Id,
                };
                
                var result = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                    await userManager.AddToRoleAsync(newAdminUser, "Employee");
                }
            }
        }

        public static async Task SeedDepartmentsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Depts.AnyAsync())
            {
                var addresses = await context.Addresses.ToListAsync();
                var directors = await context.Emps.Take(3).ToListAsync();
                
                if (addresses.Count >= 3)
                {
                    var departments = new List<Dept>
                    {
                        new Dept { AddressId = addresses[0].Id, DirectorId = directors.Count > 0 ? directors[0].Id : null },
                        new Dept { AddressId = addresses[1].Id, DirectorId = directors.Count > 1 ? directors[1].Id : null },
                        new Dept { AddressId = addresses[2].Id, DirectorId = directors.Count > 2 ? directors[2].Id : null }
                    };
                    
                    await context.Depts.AddRangeAsync(departments);
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedBooksAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Books.AnyAsync())
            {
                var publishers = await context.Publishers.ToListAsync();
                var authors = await context.Authors.ToListAsync();
                var genres = await context.Genres.ToListAsync();
                var languages = await context.Languages.ToListAsync();
                var availableState = await context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                var departments = await context.Depts.ToListAsync();

                var books = new List<Book>
                {
                    new Book { Title = "Wiedźmin - Ostatnie życzenie", Isbn = "9788375781234", PublisherId = publishers[0].Id },
                    new Book { Title = "Solaris", Isbn = "9788376480581", PublisherId = publishers[1].Id },
                    new Book { Title = "Lód", Isbn = "9788324034567", PublisherId = publishers[2].Id },
                    new Book { Title = "Księgi Jakubowe", Isbn = "9788308082345", PublisherId = publishers[3].Id },
                    new Book { Title = "Inne Pieśni", Isbn = "9788376481236", PublisherId = publishers[1].Id },
                    new Book { Title = "Władca Pierścieni", Isbn = "9788328734561", PublisherId = publishers[4].Id },
                    new Book { Title = "Gra o Tron", Isbn = "9788324045678", PublisherId = publishers[0].Id },
                    new Book { Title = "Lśnienie", Isbn = "9788376480895", PublisherId = publishers[1].Id },
                    new Book { Title = "Diuna", Isbn = "9788376481237", PublisherId = publishers[5].Id },
                    new Book { Title = "Fundacja", Isbn = "9788376481238", PublisherId = publishers[6].Id }
                };

                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();

                // Przypisz autorów do książek
                var bookAuthors = new List<BookAuthor>
                {
                    new BookAuthor { BookId = books[0].Id, AuthorId = authors[0].Id }, // Sapkowski
                    new BookAuthor { BookId = books[1].Id, AuthorId = authors[1].Id }, // Lem
                    new BookAuthor { BookId = books[2].Id, AuthorId = authors[2].Id }, // Dukaj
                    new BookAuthor { BookId = books[3].Id, AuthorId = authors[3].Id }, // Tokarczuk
                    new BookAuthor { BookId = books[4].Id, AuthorId = authors[2].Id }, // Dukaj
                    new BookAuthor { BookId = books[5].Id, AuthorId = authors[7].Id }, // Tolkien
                    new BookAuthor { BookId = books[6].Id, AuthorId = authors[8].Id }, // Martin
                    new BookAuthor { BookId = books[7].Id, AuthorId = authors[6].Id }, // King
                    new BookAuthor { BookId = books[8].Id, AuthorId = authors[9].Id }, // Herbert
                    new BookAuthor { BookId = books[9].Id, AuthorId = authors[10].Id }  // Asimov
                };

                await context.BookAuthors.AddRangeAsync(bookAuthors);

                // Przypisz gatunki do książek
                var bookGenres = new List<BookGenre>
                {
                    new BookGenre { BookId = books[0].Id, GenreId = genres[1].Id }, // Fantasy
                    new BookGenre { BookId = books[1].Id, GenreId = genres[0].Id }, // Sci-Fi
                    new BookGenre { BookId = books[2].Id, GenreId = genres[0].Id }, // Sci-Fi
                    new BookGenre { BookId = books[3].Id, GenreId = genres[5].Id }, // Literatura piękna
                    new BookGenre { BookId = books[4].Id, GenreId = genres[1].Id }, // Fantasy
                    new BookGenre { BookId = books[5].Id, GenreId = genres[1].Id }, // Fantasy
                    new BookGenre { BookId = books[6].Id, GenreId = genres[1].Id }, // Fantasy
                    new BookGenre { BookId = books[7].Id, GenreId = genres[3].Id }, // Horror
                    new BookGenre { BookId = books[8].Id, GenreId = genres[0].Id }, // Sci-Fi
                    new BookGenre { BookId = books[9].Id, GenreId = genres[0].Id }  // Sci-Fi
                };

                await context.BookGenres.AddRangeAsync(bookGenres);

                // Przypisz języki do książek
                var bookLanguages = new List<BookLanguage>
                {
                    new BookLanguage { BookId = books[0].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[1].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[2].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[3].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[4].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[5].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[5].Id, LanguageId = languages[1].Id }, // angielski
                    new BookLanguage { BookId = books[6].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[7].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[8].Id, LanguageId = languages[0].Id },
                    new BookLanguage { BookId = books[9].Id, LanguageId = languages[0].Id }
                };

                await context.BookLanguages.AddRangeAsync(bookLanguages);
                await context.SaveChangesAsync();

                // Dodaj egzemplarze
                if (availableState != null && departments.Any())
                {
                    var copies = new List<Copy>();
                    var random = new Random();

                    foreach (var book in books)
                    {
                        // Każda książka ma 3-5 egzemplarzy
                        int numberOfCopies = random.Next(3, 6);
                        for (int i = 0; i < numberOfCopies; i++)
                        {
                            var dept = departments[random.Next(departments.Count)];
                            copies.Add(new Copy
                            {
                                BookId = book.Id,
                                DeptId = dept.Id,
                                StateId = availableState.Id
                            });
                        }
                    }

                    await context.Copies.AddRangeAsync(copies);
                    await context.SaveChangesAsync();
                }
            }
        }

        public static async Task SeedReadersAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            if (!await context.Readers.AnyAsync())
            {
                var addresses = await context.Addresses.Skip(2).Take(5).ToListAsync();
                var readers = new List<Reader>();

                for (int i = 0; i < addresses.Count && i < 5; i++)
                {
                    var reader = new Reader
                    {
                        FirstName = i switch
                        {
                            0 => "Jan",
                            1 => "Agnieszka",
                            2 => "Michał",
                            3 => "Katarzyna",
                            _ => "Paweł"
                        },
                        LastName = i switch
                        {
                            0 => "Kowalski",
                            1 => "Nowak",
                            2 => "Wiśniewski",
                            3 => "Lewandowska",
                            _ => "Zieliński"
                        },
                        AddressId = addresses[i].Id
                    };
                    
                    readers.Add(reader);
                }

                await context.Readers.AddRangeAsync(readers);
                await context.SaveChangesAsync();

                // Dodaj przykładowego czytelnika jako użytkownika Identity
                var readerUser = await userManager.FindByEmailAsync("reader@library.com");
                if (readerUser == null)
                {
                    var firstReader = readers.FirstOrDefault();
                    var newReaderUser = new ApplicationUser
                    {
                        UserName = "reader@library.com",
                        Email = "reader@library.com",
                        EmailConfirmed = true,
                        ReaderId = firstReader?.Id
                    };
                    
                    var result = await userManager.CreateAsync(newReaderUser, "Reader123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newReaderUser, "Reader");
                    }
                }
            }
        }

        public static async Task SeedAllAsync(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
            await SeedAddressesAsync(serviceProvider);
            await SeedStatesAsync(serviceProvider);
            await SeedJobsAsync(serviceProvider);
            await SeedPublishersAsync(serviceProvider);
            await SeedGenresAsync(serviceProvider);
            await SeedLanguagesAsync(serviceProvider);
            await SeedAuthorsAsync(serviceProvider);
            await SeedAdminAsync(serviceProvider);
            await SeedDepartmentsAsync(serviceProvider);
            await SeedBooksAsync(serviceProvider);
            await SeedReadersAsync(serviceProvider);
        }
    }
}
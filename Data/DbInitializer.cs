using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using marsian_library.Models;
using Microsoft.EntityFrameworkCore;

namespace marsian_library.Data
{
    public static class DbInitializer
    {
        // ==================== RESET DATABASE ====================
        public static async Task ResetDatabaseAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Console.WriteLine("=== RESETOWANIE BAZY DANYCH ===");

            // Usuń wszystkich użytkowników
            var allUsers = await userManager.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                await userManager.DeleteAsync(user);
                Console.WriteLine($"Usunięto użytkownika: {user.Email}");
            }

            // Usuń wszystkie tabele w odpowiedniej kolejności (ze względu na klucze obce)
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Borrows\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Copies\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"BookAuthors\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"BookGenres\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"BookLanguages\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Books\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Emps\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Readers\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Depts\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Addresses\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Publishers\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Authors\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Genres\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Languages\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"States\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");
            await context.Database.ExecuteSqlRawAsync("BEGIN EXECUTE IMMEDIATE 'TRUNCATE TABLE \"Jobs\"'; EXCEPTION WHEN OTHERS THEN NULL; END;");

            Console.WriteLine("=== BAZA DANYCH WYCZYSZCZONA ===");
        }

        // ==================== SEED ROLES ====================
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Employee", "Reader", "Admin" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Utworzono rolę: {roleName}");
                }
            }
        }

        // ==================== SEED ADDRESSES ====================
        public static async Task SeedAddressesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Addresses.AnyAsync())
            {
                var addresses = new List<Address>
                {
                    new Address { City = "Forge World of Mars", Street = "Main Street", Building = "Palace 1", ZipCode = "00-000" },
                    new Address { City = "Warszawa", Street = "Marszałkowska", Building = "1", ZipCode = "00-001" },
                    new Address { City = "Kraków", Street = "Floriańska", Building = "12", Apartment = "3", ZipCode = "31-002" },
                    new Address { City = "Gdańsk", Street = "Długa", Building = "45", ZipCode = "80-003" },
                    new Address { City = "Poznań", Street = "Święty Marcin", Building = "78", Apartment = "12A", ZipCode = "61-004" },
                    new Address { City = "Wrocław", Street = "Rynek", Building = "25", Apartment = "7", ZipCode = "50-005" }
                };
                
                await context.Addresses.AddRangeAsync(addresses);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {addresses.Count} adresów");
            }
        }

        // ==================== SEED JOBS ====================
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
                Console.WriteLine($"Dodano {jobs.Count} stanowisk");
            }
        }

        // ==================== SEED STATES ====================
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
                Console.WriteLine($"Dodano {states.Count} stanów");
            }
        }

        // ==================== SEED PUBLISHERS ====================
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
                    new Publisher { Name = "Wydawnictwo Albatros" }
                };
                
                await context.Publishers.AddRangeAsync(publishers);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {publishers.Count} wydawców");
            }
        }

        // ==================== SEED GENRES ====================
        public static async Task SeedGenresAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Genres.AnyAsync())
            {
                var genres = new List<Genre>
                {
                    new Genre { Name = "Science Fiction" },
                    new Genre { Name = "Fantasy" },
                    new Genre { Name = "Kryminał" },
                    new Genre { Name = "Horror" },
                    new Genre { Name = "Literatura piękna" },
                    new Genre { Name = "Thriller" }
                };
                
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {genres.Count} gatunków");
            }
        }

        // ==================== SEED LANGUAGES ====================
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
                    new Language { Name = "Francuski" }
                };
                
                await context.Languages.AddRangeAsync(languages);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {languages.Count} języków");
            }
        }

        // ==================== SEED AUTHORS ====================
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
                    new Author { FirstName = "Stephen", LastName = "King" },
                    new Author { FirstName = "J.R.R.", LastName = "Tolkien" }
                };
                
                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {authors.Count} autorów");
            }
        }

        // ==================== SEED ADMIN ====================
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            string adminEmail = "belisarius.cawl@mechanicus.com";
            string adminPassword = "ArchmagosDominus123!";
            
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var address = await context.Addresses.FirstOrDefaultAsync();
                var job = await context.Jobs.FirstOrDefaultAsync(j => j.Name == "Director");
                
                if (address == null || job == null)
                {
                    Console.WriteLine("BŁĄD: Brak adresu lub stanowiska dla admina");
                    return;
                }

                // Utwórz pracownika
                var adminEmp = new Emp
                {
                    FirstName = "Belisarius",
                    LastName = "Cawl",
                    AddressId = address.Id,
                    JobId = job.Id,
                };

                await context.Emps.AddAsync(adminEmp);
                await context.SaveChangesAsync();

                // Utwórz użytkownika powiązanego z pracownikiem
                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    EmpId = adminEmp.Id
                };
                
                var result = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                    await userManager.AddToRoleAsync(newAdminUser, "Employee");
                    Console.WriteLine($"Utworzono admina: {adminEmail} (powiązany z pracownikiem ID={adminEmp.Id})");
                }
                else
                {
                    Console.WriteLine($"BŁĄD tworzenia admina: {string.Join(", ", result.Errors)}");
                }
            }
        }

        // ==================== SEED DEPARTMENTS ====================
        public static async Task SeedDepartmentsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Depts.AnyAsync())
            {
                var addresses = await context.Addresses.ToListAsync();
                var director = await context.Emps.FirstOrDefaultAsync(e => e.Job != null && e.Job.Name == "Director");
                
                if (addresses.Count >= 3)
                {
                    var departments = new List<Dept>
                    {
                        new Dept { AddressId = addresses[0].Id, DirectorId = director?.Id },
                        new Dept { AddressId = addresses[1].Id, DirectorId = null },
                        new Dept { AddressId = addresses[2].Id, DirectorId = null }
                    };
                    
                    await context.Depts.AddRangeAsync(departments);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {departments.Count} departamentów");
                }
            }
        }

        // ==================== SEED EMPLOYEES ====================
        public static async Task SeedEmployeesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            if (!await context.Emps.AnyAsync(e => e.FirstName != "Belisarius"))
            {
                var addresses = await context.Addresses.ToListAsync();
                var jobs = await context.Jobs.ToListAsync();
                var departments = await context.Depts.ToListAsync();
                
                if (addresses.Count >= 4 && jobs.Count >= 2 && departments.Any())
                {
                    var librarianJob = jobs.FirstOrDefault(j => j.Name == "Librarian");
                    var assistantJob = jobs.FirstOrDefault(j => j.Name == "Assistant");
                    
                    var employees = new List<(Emp emp, string email, string password, string role)>
                    {
                        (new Emp { FirstName = "Anna", LastName = "Kowalska", AddressId = addresses[0].Id, JobId = librarianJob?.Id ?? jobs[0].Id, DeptId = departments[0].Id }, 
                            "anna.kowalska@library.com", "Employee123!", "Employee"),
                        (new Emp { FirstName = "Piotr", LastName = "Nowak", AddressId = addresses[1].Id, JobId = assistantJob?.Id ?? jobs[1].Id, DeptId = departments[1].Id }, 
                            "piotr.nowak@library.com", "Employee123!", "Employee"),
                        (new Emp { FirstName = "Maria", LastName = "Wiśniewska", AddressId = addresses[2].Id, JobId = librarianJob?.Id ?? jobs[0].Id, DeptId = departments[2].Id }, 
                            "maria.wisniewska@library.com", "Employee123!", "Employee")
                    };
                    
                    foreach (var (emp, email, password, role) in employees)
                    {
                        await context.Emps.AddAsync(emp);
                        await context.SaveChangesAsync();
                        
                        var user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true,
                            EmpId = emp.Id
                        };
                        
                        var result = await userManager.CreateAsync(user, password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, role);
                            Console.WriteLine($"Utworzono pracownika: {email}");
                        }
                    }
                }
            }
        }

        // ==================== SEED READERS ====================
        public static async Task SeedReadersAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            if (!await context.Readers.AnyAsync())
            {
                var addresses = await context.Addresses.Skip(2).Take(4).ToListAsync();
                
                if (addresses.Count >= 4)
                {
                    var readers = new List<(Reader reader, string email, string password)>
                    {
                        (new Reader { FirstName = "Jan", LastName = "Kowalski", AddressId = addresses[0].Id }, 
                            "jan.kowalski@reader.com", "Reader123!"),
                        (new Reader { FirstName = "Agnieszka", LastName = "Nowak", AddressId = addresses[1].Id }, 
                            "agnieszka.nowak@reader.com", "Reader123!"),
                        (new Reader { FirstName = "Michał", LastName = "Wiśniewski", AddressId = addresses[2].Id }, 
                            "michal.wisniewski@reader.com", "Reader123!"),
                        (new Reader { FirstName = "Katarzyna", LastName = "Lewandowska", AddressId = addresses[3].Id }, 
                            "katarzyna.lewandowska@reader.com", "Reader123!")
                    };
                    
                    foreach (var (reader, email, password) in readers)
                    {
                        await context.Readers.AddAsync(reader);
                        await context.SaveChangesAsync();
                        
                        var user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            EmailConfirmed = true,
                            ReaderId = reader.Id
                        };
                        
                        var result = await userManager.CreateAsync(user, password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Reader");
                            Console.WriteLine($"Utworzono czytelnika: {email}");
                        }
                    }
                }
            }
        }

        // ==================== SEED BOOKS ====================
        public static async Task SeedBooksAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Books.AnyAsync())
            {
                var publishers = await context.Publishers.ToListAsync();
                var authors = await context.Authors.ToListAsync();
                var genres = await context.Genres.ToListAsync();
                var languages = await context.Languages.ToListAsync();

                if (publishers.Count >= 4 && authors.Count >= 4)
                {
                    var books = new List<Book>
                    {
                        new Book { Title = "Wiedźmin - Ostatnie życzenie", Isbn = "9788375781234", PublisherId = publishers[0].Id },
                        new Book { Title = "Solaris", Isbn = "9788376480581", PublisherId = publishers[1].Id },
                        new Book { Title = "Lód", Isbn = "9788324034567", PublisherId = publishers[2].Id },
                        new Book { Title = "Księgi Jakubowe", Isbn = "9788308082345", PublisherId = publishers[3].Id },
                        new Book { Title = "Lśnienie", Isbn = "9788376480895", PublisherId = publishers[1].Id },
                        new Book { Title = "Władca Pierścieni", Isbn = "9788328734561", PublisherId = publishers[4].Id }
                    };
                    
                    await context.Books.AddRangeAsync(books);
                    await context.SaveChangesAsync();

                    // Przypisz relacje
                    books[0].Authors.Add(authors[0]);
                    books[0].Genres.Add(genres[1]);
                    books[0].Languages.Add(languages[0]);

                    if (authors.Count > 1) books[1].Authors.Add(authors[1]);
                    if (genres.Count > 0) books[1].Genres.Add(genres[0]);
                    if (languages.Count > 1) books[1].Languages.Add(languages[1]);

                    if (authors.Count > 2) books[2].Authors.Add(authors[2]);
                    if (genres.Count > 0) books[2].Genres.Add(genres[0]);
                    if (languages.Count > 0) books[2].Languages.Add(languages[0]);

                    if (authors.Count > 3) books[3].Authors.Add(authors[3]);
                    if (genres.Count > 4) books[3].Genres.Add(genres[4]);
                    if (languages.Count > 0) books[3].Languages.Add(languages[0]);

                    if (authors.Count > 4) books[4].Authors.Add(authors[4]);
                    if (genres.Count > 3) books[4].Genres.Add(genres[3]);
                    if (genres.Count > 2) books[4].Genres.Add(genres[2]);
                    if (languages.Count > 0) books[4].Languages.Add(languages[0]);

                    if (authors.Count > 5) books[5].Authors.Add(authors[5]);
                    if (genres.Count > 1) books[5].Genres.Add(genres[1]);
                    if (languages.Count > 0) books[5].Languages.Add(languages[0]);

                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {books.Count} książek z relacjami");
                }
            }
        }

        // ==================== SEED COPIES ====================
        public static async Task SeedCopiesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Copies.AnyAsync())
            {
                var books = await context.Books.ToListAsync();
                var departments = await context.Depts.ToListAsync();
                var availableState = await context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                
                if (books.Any() && departments.Any() && availableState != null)
                {
                    var copies = new List<Copy>();
                    foreach (var book in books)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            copies.Add(new Copy
                            {
                                BookId = book.Id,
                                DeptId = departments[i % departments.Count].Id,
                                StateId = availableState.Id
                            });
                        }
                    }
                    
                    await context.Copies.AddRangeAsync(copies);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {copies.Count} egzemplarzy");
                }
            }
        }

        // ==================== SEED BORROWS ====================
        public static async Task SeedBorrowsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            
            if (!await context.Borrows.AnyAsync())
            {
                var copies = await context.Copies.ToListAsync();
                var readers = await context.Readers.ToListAsync();
                var borrowedState = await context.States.FirstOrDefaultAsync(s => s.Name == "Borrowed");
                
                if (copies.Any() && readers.Any() && borrowedState != null)
                {
                    var borrows = new List<Borrow>();
                    var random = new Random();
                    
                    // Aktywne wypożyczenie dla pierwszego czytelnika
                    if (copies.Count > 0 && readers.Count > 0)
                    {
                        borrows.Add(new Borrow
                        {
                            CopyId = copies[0].Id,
                            ReaderId = readers[0].Id,
                            BorrowDate = DateTime.Now.AddDays(-5),
                            ExpectedReturnDate = DateTime.Now.AddDays(9),
                            TimesExtended = 0
                        });
                        
                        // Zmień stan egzemplarza na wypożyczony
                        copies[0].StateId = borrowedState.Id;
                    }
                    
                    if (borrows.Any())
                    {
                        await context.Borrows.AddRangeAsync(borrows);
                        await context.SaveChangesAsync();
                        Console.WriteLine($"Dodano {borrows.Count} wypożyczeń");
                    }
                }
            }
        }

        // ==================== SEED ALL ====================
        public static async Task SeedAllAsync(IServiceProvider serviceProvider)
        {
            await SeedRolesAsync(serviceProvider);
            await SeedAddressesAsync(serviceProvider);
            await SeedJobsAsync(serviceProvider);
            await SeedStatesAsync(serviceProvider);
            await SeedPublishersAsync(serviceProvider);
            await SeedGenresAsync(serviceProvider);
            await SeedLanguagesAsync(serviceProvider);
            await SeedAuthorsAsync(serviceProvider);
            await SeedAdminAsync(serviceProvider);
            await SeedDepartmentsAsync(serviceProvider);
            await SeedEmployeesAsync(serviceProvider);
            await SeedReadersAsync(serviceProvider);
            await SeedBooksAsync(serviceProvider);
            await SeedCopiesAsync(serviceProvider);
            await SeedBorrowsAsync(serviceProvider);
            
            Console.WriteLine("=== SEEDOWANIE ZAKOŃCZONE POMYŚLNIE ===");
        }

        // ==================== RESET AND RESEED ====================
        public static async Task ResetAndReseedAsync(IServiceProvider serviceProvider)
        {
            await ResetDatabaseAsync(serviceProvider);
            await SeedAllAsync(serviceProvider);
        }
    }
}
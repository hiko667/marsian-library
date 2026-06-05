using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using marsian_library.Models;
using Microsoft.EntityFrameworkCore;

namespace marsian_library.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAllAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Console.WriteLine("=== ROZPOCZĘTO SEEDOWANIE DANYCH ===");

            // ==================== ROLES ====================
            string[] roleNames = { "Employee", "Reader", "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Utworzono rolę: {roleName}");
                }
            }

            // ==================== ADDRESSES ====================
            // Sprawdź czy mamy wystarczająco adresów, jeśli nie - dodaj
            var existingAddresses = await context.Addresses.ToListAsync();
            var requiredAddresses = new List<Address>
            {
                new Address { City = "Forge World of Mars", Street = "Main Street", Building = "Palace 1", ZipCode = "00-000" },
                new Address { City = "Warszawa", Street = "Marszałkowska", Building = "1", ZipCode = "00-001" },
                new Address { City = "Kraków", Street = "Floriańska", Building = "12", Apartment = "3", ZipCode = "31-002" },
                new Address { City = "Gdańsk", Street = "Długa", Building = "45", ZipCode = "80-003" },
                new Address { City = "Poznań", Street = "Święty Marcin", Building = "78", Apartment = "12A", ZipCode = "61-004" },
                new Address { City = "Wrocław", Street = "Rynek", Building = "25", Apartment = "7", ZipCode = "50-005" },
                new Address { City = "Łódź", Street = "Piotrkowska", Building = "89", ZipCode = "90-006" },
                new Address { City = "Katowice", Street = "Mariacka", Building = "34", Apartment = "5", ZipCode = "40-007" }
            };

            if (existingAddresses.Count < requiredAddresses.Count)
            {
                var newAddresses = requiredAddresses.Skip(existingAddresses.Count).ToList();
                await context.Addresses.AddRangeAsync(newAddresses);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newAddresses.Count} adresów (teraz łącznie: {await context.Addresses.CountAsync()})");
            }

            // ==================== STATES ====================
            var existingStates = await context.States.ToListAsync();
            var requiredStates = new List<State>
            {
                new State { Name = "Available" },
                new State { Name = "Borrowed" },
                new State { Name = "Damaged" },
                new State { Name = "Lost" },
                new State { Name = "In Repair" }
            };

            if (existingStates.Count < requiredStates.Count)
            {
                var newStates = requiredStates.Skip(existingStates.Count).ToList();
                await context.States.AddRangeAsync(newStates);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newStates.Count} stanów");
            }

            // ==================== JOBS ====================
            var existingJobs = await context.Jobs.ToListAsync();
            var requiredJobs = new List<Job>
            {
                new Job { Name = "Director" },
                new Job { Name = "Librarian" },
                new Job { Name = "Assistant" },
                new Job { Name = "Technician" },
                new Job { Name = "Administrator" }
            };

            if (existingJobs.Count < requiredJobs.Count)
            {
                var newJobs = requiredJobs.Skip(existingJobs.Count).ToList();
                await context.Jobs.AddRangeAsync(newJobs);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newJobs.Count} stanowisk (teraz łącznie: {await context.Jobs.CountAsync()})");
            }

            // ==================== PUBLISHERS ====================
            var existingPublishers = await context.Publishers.ToListAsync();
            var requiredPublishers = new List<Publisher>
            {
                new Publisher { Name = "Wydawnictwo Literackie" },
                new Publisher { Name = "Prószyński i S-ka" },
                new Publisher { Name = "Znak" },
                new Publisher { Name = "Muza SA" },
                new Publisher { Name = "Wydawnictwo Albatros" },
                new Publisher { Name = "Wydawnictwo Mag" },
                new Publisher { Name = "Solaris" }
            };

            if (existingPublishers.Count < requiredPublishers.Count)
            {
                var newPublishers = requiredPublishers.Skip(existingPublishers.Count).ToList();
                await context.Publishers.AddRangeAsync(newPublishers);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newPublishers.Count} wydawców");
            }

            // ==================== GENRES ====================
            var existingGenres = await context.Genres.ToListAsync();
            var requiredGenres = new List<Genre>
            {
                new Genre { Name = "Science Fiction", ChildrenFriendly = false },
                new Genre { Name = "Fantasy", ChildrenFriendly = true },
                new Genre { Name = "Kryminał", ChildrenFriendly = false },
                new Genre { Name = "Horror", ChildrenFriendly = false },
                new Genre { Name = "Literatura piękna", ChildrenFriendly = false },
                new Genre { Name = "Thriller", ChildrenFriendly = false }
            };

            if (existingGenres.Count < requiredGenres.Count)
            {
                var newGenres = requiredGenres.Skip(existingGenres.Count).ToList();
                await context.Genres.AddRangeAsync(newGenres);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newGenres.Count} gatunków");
            }

            // ==================== LANGUAGES ====================
            var existingLanguages = await context.Languages.ToListAsync();
            var requiredLanguages = new List<Language>
            {
                new Language { Name = "Polski" },
                new Language { Name = "Angielski" },
                new Language { Name = "Hiszpański" },
                new Language { Name = "Francuski" },
                new Language { Name = "Niemiecki" }
            };

            if (existingLanguages.Count < requiredLanguages.Count)
            {
                var newLanguages = requiredLanguages.Skip(existingLanguages.Count).ToList();
                await context.Languages.AddRangeAsync(newLanguages);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newLanguages.Count} języków");
            }

            // ==================== AUTHORS ====================
            var existingAuthors = await context.Authors.ToListAsync();
            var requiredAuthors = new List<Author>
            {
                new Author { FirstName = "Andrzej", LastName = "Sapkowski" },
                new Author { FirstName = "Stanisław", LastName = "Lem" },
                new Author { FirstName = "Jacek", LastName = "Dukaj" },
                new Author { FirstName = "Olga", LastName = "Tokarczuk" },
                new Author { FirstName = "Stephen", LastName = "King" },
                new Author { FirstName = "J.R.R.", LastName = "Tolkien" },
                new Author { FirstName = "Frank", LastName = "Herbert" },
                new Author { FirstName = "Isaac", LastName = "Asimov" }
            };

            if (existingAuthors.Count < requiredAuthors.Count)
            {
                var newAuthors = requiredAuthors.Skip(existingAuthors.Count).ToList();
                await context.Authors.AddRangeAsync(newAuthors);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {newAuthors.Count} autorów");
            }

            // ==================== RE-READ DATA ====================
            var allAddresses = await context.Addresses.ToListAsync();
            var allStates = await context.States.ToListAsync();
            var allJobs = await context.Jobs.ToListAsync();
            var allPublishers = await context.Publishers.ToListAsync();
            var allGenres = await context.Genres.ToListAsync();
            var allLanguages = await context.Languages.ToListAsync();
            var allAuthors = await context.Authors.ToListAsync();

            Console.WriteLine($"Odczytano dane: Addresses={allAddresses.Count}, States={allStates.Count}, Jobs={allJobs.Count}, Publishers={allPublishers.Count}, Genres={allGenres.Count}, Languages={allLanguages.Count}, Authors={allAuthors.Count}");

            var availableState = allStates.FirstOrDefault(s => s.Name == "Available");
            var borrowedState = allStates.FirstOrDefault(s => s.Name == "Borrowed");
            var directorJob = allJobs.FirstOrDefault(j => j.Name == "Director");
            var librarianJob = allJobs.FirstOrDefault(j => j.Name == "Librarian");
            var assistantJob = allJobs.FirstOrDefault(j => j.Name == "Assistant");

            if (availableState == null)
            {
                Console.WriteLine("BŁĄD: Brak stanu 'Available' w bazie!");
                return;
            }

            // ==================== EMPS & DEPTS ====================
            if (!await context.Emps.AnyAsync())
            {
                var empList = new List<Emp>();

                if (directorJob != null && allAddresses.Count > 0)
                {
                    empList.Add(new Emp { FirstName = "Anna", LastName = "Kowalska", AddressId = allAddresses[0].Id, JobId = directorJob.Id });
                    empList.Add(new Emp { FirstName = "Maria", LastName = "Wiśniewska", AddressId = allAddresses[2].Id, JobId = directorJob.Id });
                }

                if (librarianJob != null && allAddresses.Count > 1)
                {
                    empList.Add(new Emp { FirstName = "Piotr", LastName = "Nowak", AddressId = allAddresses[1].Id, JobId = librarianJob.Id });
                }

                if (assistantJob != null && allAddresses.Count > 3)
                {
                    empList.Add(new Emp { FirstName = "Tomasz", LastName = "Lewandowski", AddressId = allAddresses[3].Id, JobId = assistantJob.Id });
                }

                if (empList.Any())
                {
                    await context.Emps.AddRangeAsync(empList);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {empList.Count} pracowników");

                    // Departamenty
                    var deptList = new List<Dept>();
                    
                    if (allAddresses.Count > 0 && empList.Count > 0)
                    {
                        deptList.Add(new Dept { AddressId = allAddresses[0].Id, DirectorId = empList[0].Id });
                    }
                    if (allAddresses.Count > 1 && empList.Count > 1)
                    {
                        deptList.Add(new Dept { AddressId = allAddresses[1].Id, DirectorId = empList[1]?.Id });
                    }
                    if (allAddresses.Count > 2)
                    {
                        deptList.Add(new Dept { AddressId = allAddresses[2].Id, DirectorId = null });
                    }

                    await context.Depts.AddRangeAsync(deptList);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {deptList.Count} departamentów");

                    // Aktualizuj pracowników o DeptId
                    var allEmps = await context.Emps.ToListAsync();
                    var allDepts = await context.Depts.ToListAsync();
                    
                    for (int i = 0; i < allEmps.Count && i < allDepts.Count; i++)
                    {
                        allEmps[i].DeptId = allDepts[i % allDepts.Count].Id;
                    }
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Zaktualizowano DeptId pracowników");
                }
            }

            // ==================== READERS ====================
            if (!await context.Readers.AnyAsync() && allAddresses.Count >= 6)
            {
                var readers = new List<Reader>
                {
                    new Reader { FirstName = "Jan", LastName = "Kowalski", AddressId = allAddresses[3].Id },
                    new Reader { FirstName = "Agnieszka", LastName = "Nowak", AddressId = allAddresses[4].Id },
                    new Reader { FirstName = "Michał", LastName = "Wiśniewski", AddressId = allAddresses[5].Id }
                };
                await context.Readers.AddRangeAsync(readers);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {readers.Count} czytelników");
            }

            // ==================== BOOKS ====================
            if (!await context.Books.AnyAsync() && allPublishers.Count >= 5)
            {
                var books = new List<Book>
                {
                    new Book { Title = "Wiedźmin - Ostatnie życzenie", Isbn = "9788375781234", PublisherId = allPublishers[0].Id },
                    new Book { Title = "Solaris", Isbn = "9788376480581", PublisherId = allPublishers[1].Id },
                    new Book { Title = "Lód", Isbn = "9788324034567", PublisherId = allPublishers[2].Id },
                    new Book { Title = "Księgi Jakubowe", Isbn = "9788308082345", PublisherId = allPublishers[3].Id },
                    new Book { Title = "Lśnienie", Isbn = "9788376480895", PublisherId = allPublishers[1].Id },
                    new Book { Title = "Władca Pierścieni", Isbn = "9788328734561", PublisherId = allPublishers[4].Id }
                };
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
                Console.WriteLine($"Dodano {books.Count} książek");

                // Dodaj relacje
                if (allAuthors.Count > 0 && allGenres.Count > 1 && allLanguages.Count > 0)
                {
                    books[0].Authors.Add(allAuthors[0]);
                    books[0].Genres.Add(allGenres[1]);
                    books[0].Languages.Add(allLanguages[0]);

                    if (allAuthors.Count > 1) books[1].Authors.Add(allAuthors[1]);
                    if (allGenres.Count > 0) books[1].Genres.Add(allGenres[0]);
                    if (allLanguages.Count > 1) books[1].Languages.Add(allLanguages[1]);

                    if (allAuthors.Count > 2) books[2].Authors.Add(allAuthors[2]);
                    if (allGenres.Count > 0) books[2].Genres.Add(allGenres[0]);
                    if (allLanguages.Count > 0) books[2].Languages.Add(allLanguages[0]);

                    if (allAuthors.Count > 3) books[3].Authors.Add(allAuthors[3]);
                    if (allGenres.Count > 4) books[3].Genres.Add(allGenres[4]);
                    if (allLanguages.Count > 0) books[3].Languages.Add(allLanguages[0]);

                    if (allAuthors.Count > 4) books[4].Authors.Add(allAuthors[4]);
                    if (allGenres.Count > 3) books[4].Genres.Add(allGenres[3]);
                    if (allGenres.Count > 2) books[4].Genres.Add(allGenres[2]);
                    if (allLanguages.Count > 0) books[4].Languages.Add(allLanguages[0]);

                    if (allAuthors.Count > 5) books[5].Authors.Add(allAuthors[5]);
                    if (allGenres.Count > 1) books[5].Genres.Add(allGenres[1]);
                    if (allLanguages.Count > 0) books[5].Languages.Add(allLanguages[0]);

                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano relacje dla książek");
                }
            }

            // ==================== COPIES ====================
            if (!await context.Copies.AnyAsync())
            {
                var depts = await context.Depts.ToListAsync();
                var allBooks = await context.Books.ToListAsync();
                if (depts.Any() && allBooks.Any())
                {
                    var copies = new List<Copy>();
                    foreach (var book in allBooks)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            copies.Add(new Copy
                            {
                                BookId = book.Id,
                                DeptId = depts[i % depts.Count].Id,
                                StateId = availableState.Id
                            });
                        }
                    }
                    await context.Copies.AddRangeAsync(copies);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {copies.Count} egzemplarzy");
                }
            }

            // ==================== BORROWS ====================
            if (!await context.Borrows.AnyAsync())
            {
                var readers = await context.Readers.ToListAsync();
                var copies = await context.Copies.Where(c => c.StateId == availableState.Id).Take(3).ToListAsync();

                if (readers.Any() && copies.Any())
                {
                    var borrows = new List<Borrow>
                    {
                        new Borrow
                        {
                            CopyId = copies[0].Id,
                            ReaderId = readers[0].Id,
                            BorrowDate = DateTime.Now.AddDays(-10),
                            ExpectedReturnDate = DateTime.Now.AddDays(4),
                            TimesExtended = 0
                        },
                        new Borrow
                        {
                            CopyId = copies[1].Id,
                            ReaderId = readers[1].Id,
                            BorrowDate = DateTime.Now.AddDays(-5),
                            ExpectedReturnDate = DateTime.Now.AddDays(9),
                            TimesExtended = 1
                        }
                    };

                    await context.Borrows.AddRangeAsync(borrows);
                    await context.SaveChangesAsync();

                    copies[0].StateId = borrowedState.Id;
                    copies[1].StateId = borrowedState.Id;
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Dodano {borrows.Count} wypożyczeń");
                }
            }

            // ==================== ADMIN USER ====================
            string adminEmail = "admin@library.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var adminEmp = await context.Emps.FirstOrDefaultAsync();
                var newAdminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    EmpId = adminEmp?.Id
                };
                var result = await userManager.CreateAsync(newAdminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                    await userManager.AddToRoleAsync(newAdminUser, "Employee");
                    Console.WriteLine($"Utworzono użytkownika admin: {adminEmail}");
                }
            }

            // ==================== READER USER ====================
            string readerEmail = "reader@library.com";
            var readerUser = await userManager.FindByEmailAsync(readerEmail);
            if (readerUser == null)
            {
                var firstReader = await context.Readers.FirstOrDefaultAsync();
                var newReaderUser = new ApplicationUser
                {
                    UserName = readerEmail,
                    Email = readerEmail,
                    EmailConfirmed = true,
                    ReaderId = firstReader?.Id
                };
                var result = await userManager.CreateAsync(newReaderUser, "Reader123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newReaderUser, "Reader");
                    Console.WriteLine($"Utworzono użytkownika reader: {readerEmail}");
                }
            }

            Console.WriteLine("=== ZAKOŃCZONO SEEDOWANIE DANYCH ===");
        }
    }
}
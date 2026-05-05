using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using marsian_library.Models; 

namespace marsian_library.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<BookLanguage> BookLanguages { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Dept> Depts { get; set; }
        public DbSet<Emp> Emps { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("SYSTEM");

            builder.Entity<Dept>()
                .HasOne(d => d.Director)
                .WithMany() // director is a worker too, doesn't need to have a collection of depts he oversees
                .HasForeignKey(d => d.DirectorId)
                .OnDelete(DeleteBehavior.Restrict); // do not delete emps when deleting dept

            builder.Entity<Emp>()
                .HasOne(e => e.Dept)
                .WithMany(d => d.Emps) // one dept has many emps
                .HasForeignKey(e => e.DeptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            builder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });
            
            builder.Entity<BookLanguage>()
                .HasKey(bl => new { bl.BookId, bl.LanguageId });
            
            builder.Entity<Book>()
                .HasIndex(b => b.Isbn)
                .IsUnique();
        }
    }
}
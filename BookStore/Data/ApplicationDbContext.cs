using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", schema: "Identity");
            builder.Entity<IdentityRole>().ToTable("Roles", schema: "Identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schema: "Identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schema: "Identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", schema: "Identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", schema: "Identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", schema: "Identity");


            builder.Entity<Author>().ToTable("Authors", schema: "bk");
            builder.Entity<Book>().ToTable("Books", schema: "bk");
            builder.Entity<BookReader>().ToTable("BookReaders", schema: "bk");
            builder.Entity<Category>().ToTable("Categories", schema: "bk");


            builder
                .Entity<Book>()
                .HasOne(i=>i.Publisher)
                .WithMany(ii=>ii.PublishedBooks)
                .HasForeignKey(iii=>iii.PublisherId);

            builder
                .Entity<Book>()
                .HasOne(i => i.Author)
                .WithMany(ii => ii.Books)
                .HasForeignKey(iii => iii.AuthorId);


            builder
                .Entity<Book>()
                .HasOne(i => i.Category)
                .WithMany(ii => ii.Books)
                .HasForeignKey(iii => iii.CategoryId);

            builder
                .Entity<Book>()
                .HasOne(i => i.Category)
                .WithMany(ii => ii.Books)
                .HasForeignKey(iii => iii.CategoryId);

            builder
                .Entity<Book>()
                .Property(i => i.PublishDate)
                .HasDefaultValue(DateTime.Now);


            builder
                .Entity<Book>()
                .Property(i => i.Name)
                .IsRequired();

            builder.Entity<Book>()
                .HasMany(i => i.Readers)
                .WithMany(ii => ii.ReadBooks)
                .UsingEntity<BookReader>
                (
                    br =>
                    {
                        br
                        .HasOne(bri => bri.Book)
                        .WithMany(brb=>brb.BookReaders)
                        .HasForeignKey(bri => bri.BookId)
                        .OnDelete(DeleteBehavior.NoAction);

                        br
                        .HasOne(bri => bri.user)
                        .WithMany(brb => brb.BookReaders)
                        .HasForeignKey(bri => bri.userId)
                        .OnDelete(DeleteBehavior.NoAction);

                        br
                        .Property(bri => bri.Rate)
                        .HasDefaultValue(0.0)
                        .HasColumnType("decimal(18, 6)");
                        
                        br
                        .HasKey(bri => new { bri.BookId, bri.userId});
                    }
                );

        }


        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookReader> BookReaders { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

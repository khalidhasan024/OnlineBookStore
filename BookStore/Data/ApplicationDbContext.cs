using System;
using System.Collections.Generic;
using System.Text;
using BookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<BookCategory>()
                .HasMany(c => c.Books)
                .WithOne(b => b.Category)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<JoinBookTag>()
                .HasKey(bt => new { bt.BookId, bt.TagId });
            builder.Entity<JoinBookTag>()
                .HasOne(bt => bt.Book)
                .WithMany(b => b.BookTags)
                .HasForeignKey(t => t.BookId);
            builder.Entity<JoinBookTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BookTags)
                .HasForeignKey(b => b.TagId);

            builder.Entity<BookOrder>()
                .HasKey(bo => new { bo.BookId, bo.OrderId });
            builder.Entity<BookOrder>()
                .HasOne(bo => bo.Book)
                .WithMany(b => b.BookOrders)
                .HasForeignKey(o => o.BookId);
            builder.Entity<BookOrder>()
                .HasOne(bo => bo.Order)
                .WithMany(o => o.BookOrders)
                .HasForeignKey(b => b.OrderId);

        }
    }
}

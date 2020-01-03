using LibraryData2.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryData2
{
    public class LibraryContext:DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public DbSet<LibraryBranch> LibraryBranchs { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<Patron> Patrons { get; set; }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<LibraryAsset> libraryAssets{ get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<BranchHours> BranchHourss { get; set; }
    }
}

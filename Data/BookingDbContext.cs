using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {

    }

    //Table
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    //Other Configuration or Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Unique
        modelBuilder.Entity<Employee>()
                    .HasIndex(e => new
                    {
                        e.Email,
                        e.NIK,
                        e.PhoneNumber
                    }).IsUnique();

        //University - Education (One to Many)
        modelBuilder.Entity<University>()
                    .HasMany(university => university.Educations)
                    .WithOne(education => education.Universities)
                    .HasForeignKey(education => education.UniversityGuid);

        //Room - Booking (One to Many)
        modelBuilder.Entity<Room>()
                    .HasMany(room => room.Bookings)
                    .WithOne(booking => booking.Rooms)
                    .HasForeignKey(booking => booking.RoomGuid);

        //Employee - Education (One to One)
        modelBuilder.Entity<Education>()
                    .HasOne(education => education.Employees)
                    .WithOne(employee => employee.Educations)
                    .HasForeignKey<Education>(education => education.Guid);

        //Employee - Booking (One to Many)
        modelBuilder.Entity<Employee>()
                    .HasMany(employee => employee.Bookings)
                    .WithOne(booking => booking.Employees)
                    .HasForeignKey(booking => booking.EmployeeGuid);

        //Role - AccountRole (One to Many)
        modelBuilder.Entity<Role>()
                    .HasMany(role => role.AccountRoles)
                    .WithOne(accountRole => accountRole.Roles)
                    .HasForeignKey(accountRole => accountRole.RoleGuid);

        //Account - AccountRole (One to Many)
        modelBuilder.Entity<Account>()
                    .HasMany(account => account.AccountRoles)
                    .WithOne(accountRole => accountRole.Accounts)
                    .HasForeignKey(accountRole => accountRole.AccountGuid);

        //Account - Employee (One to One)
        modelBuilder.Entity<Account>()
                    .HasOne(account => account.Employees)
                    .WithOne(employee => employee.Accounts)
                    .HasForeignKey<Account>(account => account.Guid);
    }

}

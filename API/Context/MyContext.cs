using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasOne(e => e.Account).WithOne(a => a.Employee).HasForeignKey<Account>(a => a.NIK);

            modelBuilder.Entity<Account>().HasOne(a => a.Profiling).WithOne(p => p.Account).HasForeignKey<Profiling>(p => p.NIK);

            modelBuilder.Entity<Education>().HasMany(e => e.Profilings).WithOne(p => p.Education);

            modelBuilder.Entity<University>().HasMany(u => u.Educations).WithOne(e => e.University);

            modelBuilder.Entity<AccountRole>().HasKey(ar => ar.Id);

            modelBuilder.Entity<AccountRole>().HasOne(ar => ar.Account).WithMany(a => a.AccountRoles).HasForeignKey(ar => ar.NIK);

            modelBuilder.Entity<AccountRole>().HasOne(ar => ar.Role).WithMany(r => r.AccountRoles).HasForeignKey(ar => ar.RoleId);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
    }
}

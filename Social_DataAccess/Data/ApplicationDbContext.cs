﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social_Models;

namespace Social_DataAccess;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationUserActivity> ApplicationUserActivities { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<ApplicationUserActivity>()
        .HasKey(ua => new { ua.ApplicationUserId, ua.ActivityId });

    modelBuilder.Entity<ApplicationUserActivity>()
        .HasOne(ua => ua.Activity)
        .WithMany(a => a.ApplicationUserActivities)
        .HasForeignKey(ua => ua.ActivityId);

    modelBuilder.Entity<Activity>().HasData(
        new Activity
        {
            Id = 1,
            Title = "Spela Fotboll",
            CreatedBy = "Omar",
            Email = "Omar@example.com",
            BirthDate = DateTime.Parse("1991-10-09"),
            City = "Bollebygd",
            Address = "B-Vallen",
            Date = new DateTime(2024, 7, 24)
        },
        new Activity
        {
            Id = 2,
            Title = "Spela Golf",
            CreatedBy = "Fighter",
            Email = "Fighter@example.com",
            BirthDate = DateTime.Parse("1992-07-14"),
            City = "Bollebygd",
            Address = "Hulta",
            Date = new DateTime(2024, 7, 24)
        },
        new Activity
        {
            Id = 3,
            Title = "Basket Match",
            CreatedBy = "Bob",
            Email = "Bob@example.com",
            BirthDate = DateTime.Parse("1993-04-28"),
            City = "Bollebygd",
            Address = "Bollebygd skolan",
            Date = new DateTime(2024, 7, 24)
        }
    );

    modelBuilder.Entity<ApplicationUser>().HasData(
        new ApplicationUser()
        {
            FullName = "Omar",
            BirthDate = DateTime.Parse("1991-10-09"),
            Email = "Omar@example.com",
            Address = "Adress1",
            City = "City1",
            PostalCode = "1",
        },
        new ApplicationUser()
        {
            FullName = "Bob",
            BirthDate = DateTime.Parse("2015-07-14"),
            Email = "Bob@example.com",
            Address = "Adress2",
            City = "City2",
            PostalCode = "2",
        },
        new ApplicationUser()
        {
            FullName = "Fighter",
            BirthDate = DateTime.Parse("2022-04-28"),
            Email = "Fighter@example.com",
            Address = "Adress3",
            City = "City3",
            PostalCode = "3",
        }
    );
}
}
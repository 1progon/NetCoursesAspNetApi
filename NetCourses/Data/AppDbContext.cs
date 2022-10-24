using Microsoft.EntityFrameworkCore;
using NetCourses.Models;
using NetCourses.Models.Companies;
using NetCourses.Models.Courses;
using NetCourses.Models.Jobs;

namespace NetCourses.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Job>()
            .HasOne(j => j.Paid)
            .WithOne(p => p.Job)
            .HasForeignKey<JobsPaid>(p => p.JobId);
    }

    // Jobs
    public DbSet<Job> Jobs { get; set; } = null!;

    // Companies
    public DbSet<Company> Companies { get; set; } = null!;

    // Courses
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<CourseVideos> CoursesVideos { get; set; } = null!;

    // languages
    public DbSet<Language> Languages { get; set; } = null!;
}
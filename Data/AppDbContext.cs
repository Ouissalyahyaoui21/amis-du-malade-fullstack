using Microsoft.EntityFrameworkCore;
using AmisduMalade.Models;

namespace AmisduMalade.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        // الجداول الأساسية
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<PatientRequest> PatientRequests { get; set; }
        public DbSet<Admin> Admins { get; set; }
        
        // الجداول الجديدة
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingEnrollment> TrainingEnrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Center> Centers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // بيانات أولية - مراكز الجمعية
            modelBuilder.Entity<Center>().HasData(
                new Center { Id=1, Name="مركز سكيكدة", Municipality="سكيكدة", Address="وسط المدينة" },
                new Center { Id=2, Name="مركز عزابة", Municipality="عزابة", Address="حي النصر" },
                new Center { Id=3, Name="مركز الحروش", Municipality="الحروش", Address="الشارع الرئيسي" },
                new Center { Id=4, Name="مركز القل", Municipality="القل", Address="وسط البلدية" },
                new Center { Id=5, Name="مركز الميلية", Municipality="الميلية", Address="حي السلام" }
            );
        }
    }
}
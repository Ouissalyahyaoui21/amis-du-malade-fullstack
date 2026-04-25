using Microsoft.EntityFrameworkCore;
using AmisduMalade.Models;

namespace AmisduMalade.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Administration
        public DbSet<AssociationBranch> AssociationBranches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }

        // Patients
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientContact> PatientContacts { get; set; }
        public DbSet<MedicalCondition> MedicalConditions { get; set; }
        public DbSet<PatientMedicalCondition> PatientMedicalConditions { get; set; }

        // Volunteers
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<VolunteerSkill> VolunteerSkills { get; set; }
        public DbSet<VolunteerAvailability> VolunteerAvailabilities { get; set; }
        public DbSet<VolunteerCoverageArea> VolunteerCoverageAreas { get; set; }
        public DbSet<VolunteerDocument> VolunteerDocuments { get; set; }
        public DbSet<VolunteerInterview> VolunteerInterviews { get; set; }

        // Training
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingEnrollment> TrainingEnrollments { get; set; }

        // Operations
        public DbSet<CareRequest> CareRequests { get; set; }
        public DbSet<CareRequestRequiredSkill> CareRequestRequiredSkills { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<VisitSession> VisitSessions { get; set; }
        public DbSet<VisitNote> VisitNotes { get; set; }
        public DbSet<VisitRating> VisitRatings { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        // System
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data - مراكز الجمعية
            modelBuilder.Entity<AssociationBranch>().HasData(
                new AssociationBranch
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "مركز سكيكدة",
                    Municipality = "سكيكدة",
                    Address = "وسط المدينة"
                },
                new AssociationBranch
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "مركز عزابة",
                    Municipality = "عزابة",
                    Address = "حي النصر"
                }
            );

            // Seed data - المهارات
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "رعاية المريض", Category = "رعاية" },
                new Skill { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "نقل المرضى", Category = "نقل" },
                new Skill { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "مساعدة طبية", Category = "طبي" },
                new Skill { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "الدعم النفسي", Category = "نفسي" },
                new Skill { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "تحضير الطعام", Category = "منزلي" }
            );

            // Seed data - الأمراض
            modelBuilder.Entity<MedicalCondition>().HasData(
                new MedicalCondition { Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "مريض بالسكري", Category = "مزمن" },
                new MedicalCondition { Id = Guid.Parse("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "أمراض القلب", Category = "قلبي" },
                new MedicalCondition { Id = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "مريض بالسرطان", Category = "سرطان" },
                new MedicalCondition { Id = Guid.Parse("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "شلل / إعاقة حركية", Category = "حركي" }
            );
        }
    }
}
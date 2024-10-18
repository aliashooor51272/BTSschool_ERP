using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.DataAccess.HRModels;
using ByteTechSchoolERP.Models.Students;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ByteTechSchoolERP.Models.Hostel;
using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models.Lesson;
using ByteTechSchoolERP.Models.InstitudesProfile;
using ByteTechSchoolERP.Models.Campus;
using ByteTechSchoolERP.Models.SubAdmin;
using ByteTechSchoolERP.Models.Topic;
using System.Collections.Generic;
using System.Diagnostics;
using ByteTechSchoolERP.Models.Exam;
using ByteTechSchoolERP.Models.HR;
using ByteTechSchoolERP.Models.Marks;
using ByteTechSchoolERP.Models.Parents;
using ByteTechSchoolERP.Models.HomeDiary;
using ByteTechSchoolERP.Models.TimeTable;
using ByteTechSchoolERP.Models.Accounts;
using ByteTechSchoolERP.Models.Vourchars;
namespace ByteTechSchoolERP.DataAccess.Data
{
    public class ByteTechSchoolERPContext : IdentityDbContext<ByteTechSchoolERPUser>
    {
        public ByteTechSchoolERPContext(DbContextOptions<ByteTechSchoolERPContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Payroll> Payroll { get; set; }
        public DbSet<Salary> Salary { get; set; }
        public DbSet<Loan> Loan { get; set; }
       
        public DbSet<Employee> Employee { get; set; }
        public DbSet<ExamSchedular> ExamSchedulars { get; set; }
        public DbSet<Marks> Marks { get; set; }
        public DbSet<HomeWork> HomeWork { get; set; }
        public DbSet<SubAdminn> SubAdminn { get; set; }
        public DbSet<SchoolCampus> SchoolCampus { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }

        // HR Management
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Department> Deparments { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<HostelRoomType> HostelRoomTypes { get; set; }
        public DbSet<SubjectModel> SubjectModels { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<TimetableEntries> TimetableEntries { get; set; }
        public DbSet<ClassTimetable> ClassTimetables { get; set; }

        //instituite profile
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<ExamTemplate> ExamTemplates { get; set; }
        public DbSet<ExamList> ExamLists { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StaffTemp> StaffTemps { get; set; }
        public DbSet<Diary> HomeDiary { get; set; }
        public DbSet<SubjectAllocation> SubjectAllocations { get; set; }
        public DbSet<Parent> Parents { get; set; }

        // Accounts related tables
        public DbSet<Element_Account> Element_Accounts { get; set; }
        public DbSet<Control_Account> Control_Accounts { get; set; }
        public DbSet<Ledger_Account> Ledger_Accounts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Reciept_Voucher_payment> Reciept_Voucher_Payments { get; set; }
        public DbSet<Receipt_Vouchers> Receipt_Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define Student to Class relationship (One-to-One)
            builder.Entity<Student>()
                .HasOne<Class>(s => s.Class)
                .WithMany()
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define Class to Section relationship (One-to-Many)
            builder.Entity<Section>()
                .HasOne<Class>(s => s.Class)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define Diary to Subject relationship (Many-to-One)
            builder.Entity<Diary>()
                .HasOne(d => d.Subject)
                .WithMany()
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Specify column type for Income
            builder.Entity<Student>()
                .Property(s => s.ParentIncome)
                .HasColumnType("decimal(18,2)");

            // Specify column type for BasicSalary
            builder.Entity<Staff>()
                .Property(s => s.BasicSalary)
                .HasColumnType("decimal(18,2)");

            // Account Relationships Fix for Cascading Issue
            builder.Entity<Ledger_Account>()
                .HasOne(l => l.Control_Account)
                .WithMany(c => c.Ledger_Accounts)
                .HasForeignKey(l => l.Control_AccountId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete here

            builder.Entity<Ledger_Account>()
                .HasOne(l => l.Element_Account)
                .WithMany(e => e.Ledger_Accounts)
                .HasForeignKey(l => l.Element_AccountId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete here
        }
    }
}

using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.HR;
using ByteTechSchoolERP.Models.Parents;
using ByteTechSchoolERP.Models.Students;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Utility
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUserAndRole(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ByteTechSchoolERPUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ByteTechSchoolERPContext>();

            // Role creation (Admin, SubAdmin, Teacher, Parent)
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("SubAdmin"))
                await roleManager.CreateAsync(new IdentityRole("SubAdmin"));

            if (!await roleManager.RoleExistsAsync("Teacher"))
                await roleManager.CreateAsync(new IdentityRole("Teacher"));

            if (!await roleManager.RoleExistsAsync("Parent"))
                await roleManager.CreateAsync(new IdentityRole("Parent"));

            // Class and Section seeding
            await SeedClassesAndSections(dbContext);

            // Admin user seeding
            await SeedAdminUser(userManager);

            // Teacher user seeding
            await SeedTeachers(dbContext, userManager);

            // Parent and Student seeding
            await SeedParentsAndStudents(dbContext, userManager);
        }

        private static async Task SeedClassesAndSections(ByteTechSchoolERPContext dbContext)
        {
            // Check and create Class 1
            var class1 = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassName == "Class 1");
            if (class1 == null)
            {
                class1 = new Class { ClassName = "Class 1", Description = "Description for Class 1" };
                await dbContext.Classes.AddAsync(class1);
                await dbContext.SaveChangesAsync();

                // Create and add sections for Class 1
                var sectionA = new Section { Name = "Section A", Description = "Description for Section A", ClassId = class1.Id };
                var sectionB = new Section { Name = "Section B", Description = "Description for Section B", ClassId = class1.Id };
                await dbContext.Sections.AddAsync(sectionA);
                await dbContext.Sections.AddAsync(sectionB);
                await dbContext.SaveChangesAsync();
            }

            // Check and create Class 2
            var class2 = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassName == "Class 2");
            if (class2 == null)
            {
                class2 = new Class { ClassName = "Class 2", Description = "Description for Class 2" };
                await dbContext.Classes.AddAsync(class2);
                await dbContext.SaveChangesAsync();

                // Create and add sections for Class 2
                var sectionC = new Section { Name = "Section C", Description = "Description for Section C", ClassId = class2.Id };
                var sectionD = new Section { Name = "Section D", Description = "Description for Section D", ClassId = class2.Id };
                await dbContext.Sections.AddAsync(sectionC);
                await dbContext.Sections.AddAsync(sectionD);
                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedAdminUser(UserManager<ByteTechSchoolERPUser> userManager)
        {
            var adminUser = await userManager.FindByNameAsync("admin@gmail.com");
            if (adminUser == null)
            {
                var user = new ByteTechSchoolERPUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = ""
                };

                var password = "Admin123@#$";
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    await userManager.AddToRoleAsync(user, "SubAdmin");
                }
            }
        }

        private static async Task SeedTeachers(ByteTechSchoolERPContext dbContext, UserManager<ByteTechSchoolERPUser> userManager)
        {
            var teachers = new[]
            {
                new { UserName = "johnsmith", Email = "johnsmith@example.com", FirstName = "John", LastName = "Smith", Cnic = "1234567890123", ContactNumber = "1234567890" },
                new { UserName = "emilyjones", Email = "emilyjones@example.com", FirstName = "Emily", LastName = "Jones", Cnic = "2345678901234", ContactNumber = "2345678901" },
                new { UserName = "michaelbrown", Email = "michaelbrown@example.com", FirstName = "Michael", LastName = "Brown", Cnic = "3456789012345", ContactNumber = "3456789012" },
                new { UserName = "sarahjohnson", Email = "sarahjohnson@example.com", FirstName = "Sarah", LastName = "Johnson", Cnic = "4567890123456", ContactNumber = "4567890123" },
                new { UserName = "davidwilson", Email = "davidwilson@example.com", FirstName = "David", LastName = "Wilson", Cnic = "5678901234567", ContactNumber = "5678901234" }
            };

            foreach (var teacher in teachers)
            {
                var staffTemp = dbContext.StaffTemps.FirstOrDefault(s => s.Email == teacher.Email);
                if (staffTemp == null)
                {
                    staffTemp = new StaffTemp
                    {
                        Id = Guid.NewGuid(),
                        FirstName = teacher.FirstName,
                        LastName = teacher.LastName,
                        Email = teacher.Email,
                        ContactNumber = teacher.ContactNumber,
                        Cnic = teacher.Cnic
                    };
                    dbContext.StaffTemps.Add(staffTemp);
                    await dbContext.SaveChangesAsync();
                }

                var teacherUser = await userManager.FindByEmailAsync(teacher.Email);
                if (teacherUser == null)
                {
                    teacherUser = new ByteTechSchoolERPUser
                    {
                        UserName = teacher.UserName,
                        Email = teacher.Email,
                        EmailConfirmed = true,
                        PhoneNumber = teacher.ContactNumber,
                        CNICNumber = teacher.Cnic,
                        StaffTempId = staffTemp.Id
                    };

                    var teacherPassword = "Teacher123@#$";
                    var teacherResult = await userManager.CreateAsync(teacherUser, teacherPassword);

                    if (teacherResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(teacherUser, "Teacher");
                    }
                }
            }
        }
        private static async Task SeedParentsAndStudents(ByteTechSchoolERPContext dbContext, UserManager<ByteTechSchoolERPUser> userManager)
        {
            // Define parent data
            var parentData = new[]
            {
        new { Name = "Aqibfamily", Email = "john.doe@example.com", Gender = "Male", Address = "123 Elm Street", Cnic = "1234567890123" },
        new { Name = "AhmedFamily", Email = "jane.smith@example.com", Gender = "Female", Address = "456 Oak Avenue", Cnic = "2345678901234" }
    };

            foreach (var data in parentData)
            {
                // Check if parent already exists
                var existingParent = await dbContext.Parents
                    .FirstOrDefaultAsync(p => p.Email == data.Email);

                if (existingParent == null)
                {
                    // Parent does not exist, create and add
                    var newParent = new Parent
                    {
                        Name = data.Name,
                        Email = data.Email,
                        Gender = data.Gender,
                        Address = data.Address,
                        Cnic = data.Cnic
                    };

                    // Check if user already exists
                    var user = await userManager.FindByEmailAsync(data.Email);
                    if (user == null)
                    {
                        user = new ByteTechSchoolERPUser
                        {
                            UserName = data.Email,
                            Email = data.Email,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "Parent123@#$");

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Parent");
                            newParent.UserId = user.Id;
                        }
                        else
                        {
                            // Handle user creation failure if necessary
                            throw new Exception($"Failed to create user for parent with email: {data.Email}");
                        }
                    }
                    else
                    {
                        // User already exists, use existing user's Id
                        newParent.UserId = user.Id;
                    }

                    dbContext.Parents.Add(newParent);
                }
                else
                {
                    // Parent already exists, ensure UserId is correct
                    var user = await userManager.FindByEmailAsync(data.Email);
                    if (user != null && existingParent.UserId != user.Id)
                    {
                        existingParent.UserId = user.Id;
                        dbContext.Parents.Update(existingParent);
                    }
                }
            }

            // Save changes for parents
            await dbContext.SaveChangesAsync();

            // Retrieve class and section for students
            var class1 = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassName == "Class 1");
            var sectionA = await dbContext.Sections.FirstOrDefaultAsync(s => s.Name == "Section A");

            // Define student data
            var studentData = new[]
            {
        new { FullName = "Aqib", Surname = "Azad", Gender = "Male", ParentEmail = "john.doe@example.com" },
        new { FullName = "Ali", Surname = "aHMED", Gender = "Male", ParentEmail = "jane.smith@example.com" }
    };

            foreach (var data in studentData)
            {
                // Check if student already exists
                var existingStudent = await dbContext.Students
                    .FirstOrDefaultAsync(s => s.FullName == data.FullName && s.Surname == data.Surname && s.Parent.Email == data.ParentEmail);

                if (existingStudent == null)
                {
                    // Student does not exist, create and add
                    var parent = await dbContext.Parents.FirstOrDefaultAsync(p => p.Email == data.ParentEmail);
                    if (parent == null)
                    {
                        throw new Exception($"Parent with email {data.ParentEmail} not found.");
                    }

                    var newStudent = new Student
                    {
                        FullName = data.FullName,
                        Surname = data.Surname,
                        ClassId = class1?.Id,
                        SectionId = sectionA?.Id,
                        Gender = data.Gender,
                        StudentProfileUrl = "",
                        AdmissionDate = DateTime.Now,
                        ParentId = parent.Id
                    };

                    dbContext.Students.Add(newStudent);
                }
            }

            // Save changes for students
            await dbContext.SaveChangesAsync();
        }

    }
}
 

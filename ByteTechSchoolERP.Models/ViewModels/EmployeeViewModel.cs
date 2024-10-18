using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace ByteTechSchoolERP.Models.ViewModels
{
    public class EmployeeViewModel
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FatherName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime? DateOfJoining { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? EmergencyContact { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Address { get; set; }
    public string? PermanentAddress { get; set; }
    public string? PANNumber { get; set; }
    public string? Qualification { get; set; }
    public string? WorkExperience { get; set; }
    public decimal? Salary { get; set; }
    public string DepartmentName { get; set; }
    public string RoleName { get; set; }
}
}

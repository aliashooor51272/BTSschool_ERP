using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.HRModels;
using ByteTechSchoolERP.DataAccess.ViewModels;
using ByteTechSchoolERP.Models.HR;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Data;


namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthenticationFilter]
    public class HRMController : Controller
    {

        private readonly ByteTechSchoolERPContext _context;
        private readonly RoleManager<IdentityRole> _role;
        public HRMController(ByteTechSchoolERPContext context, RoleManager<IdentityRole> role)
        {

            _role = role;
            _context = context;
        }
        // Action method to get salary data
        public async Task<IActionResult> SalaryList()
        {
            var salaries = await _context.Salary
                .Include(s => s.Employee) // Assuming Employee navigation property exists in Salary
                .Select(s => new SalaryViewModel
                {
                    SalaryId = s.SalaryId,
                    EmployeeName = s.Employee.FirstName + " " + s.Employee.LastName,
                    BasicSalary = s.BasicSalary,
                    GrossSalary = s.GrossSalary,
                    DeductionAmount = s.DeductionAmount,
                    DeductionType = s.DeductionType,
                    SalaryDate = s.SalaryDate
                })
                .ToListAsync();

            return View(salaries);
        }
        // Action to view employee loans
        public async Task<IActionResult> ViewEmployeeLoans(Guid id)
        {
            // Fetch the employee details by id
            var employee = await _context.Employee
                                         .FirstOrDefaultAsync(e => e.EmployeeId == id);

            // Fetch all loans related to the employee
            var loans = await _context.Loans
                                      .Where(l => l.EmployeeId == id)
                                      .ToListAsync();

            if (employee == null)
            {
                return NotFound();
            }

            // Create a view model to pass both employee and loan details
            var viewModel = new EmployeeLoanViewModel
            {
                Employee = employee,
                Loans = loans
            };

            // Return the view without layout
            return View("EmployeeLoanReport", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddInstallment(InstallmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the latest loan record for the current user by userId
                var existingLoan = await _context.Loan
                    .Where(l => l.EmployeeId == model.UserId)
                    .OrderByDescending(l => l.DueDate)
                    .FirstOrDefaultAsync();

                if (existingLoan != null)
                {
                    // Check if there are any remaining installments to be paid
                    if (existingLoan.RemainingInstallments > 0)
                    {
                        // Calculate new values for the next installment
                        var newRemainingInstallments = existingLoan.RemainingInstallments - 1;
                        var newRemainingAmount = existingLoan.RemainingAmount - existingLoan.PerInstallmentAmount;

                        // Update the existing loan entry
                        existingLoan.PaidInstallments += 1; // Increment paid installments
                        existingLoan.RemainingInstallments = newRemainingInstallments; // Decrement remaining installments
                        existingLoan.RemainingAmount = newRemainingAmount; // Update remaining amount
                        existingLoan.DueDate = model.DueDate; // Set the due date to the provided one (today's date)
                        existingLoan.PaidDate = DateTime.Now; // Set paid date to now

                        // Save changes to the loan record
                        _context.Loan.Update(existingLoan);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("LoanDetails");
                    }
                    else
                    {
                        return BadRequest("All installments have already been paid for this loan.");
                    }
                }
                else
                {
                    return BadRequest("Loan not found for this user.");
                }
            }

            return BadRequest("Invalid model state.");
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePayroll(DateTime payrollDate)
        {
            // Fetch employees
            var employees = await _context.Employee.ToListAsync();
            decimal totalPayrollAmount = 0;

            foreach (var employee in employees)
            {
                // Assuming that salary is fetched from Employee table or elsewhere
                var basicSalary = employee.Salary ?? 0; // Default to 0 if no salary is set
                var loan = await _context.Loans.FirstOrDefaultAsync(l => l.EmployeeId == employee.EmployeeId && l.RemainingInstallments > 0);

                decimal deductionAmount = 0;
                if (loan != null)
                {
                    deductionAmount = loan.PerInstallmentAmount;

                    // Save the updated loan record
                    loan.RemainingInstallments -= 1;
                    loan.RemainingAmount -= deductionAmount;

                    // Add a new row for loan installment history/record (loan transaction)
                    var newLoanEntry = new Loan
                    {
                        Id = Guid.NewGuid(), // New unique ID
                        EmployeeId = loan.EmployeeId,
                        LoanAmount = loan.LoanAmount,
                        NumberOfInstallments = loan.NumberOfInstallments,
                        PerInstallmentAmount = loan.PerInstallmentAmount,
                        RemainingInstallments = loan.RemainingInstallments,
                        RemainingAmount = loan.RemainingAmount,
                        DueDate = loan.DueDate,
                        PaidDate = DateTime.Now, // Current date for when the installment is paid
                        PaidInstallments = loan.PaidInstallments + 1 // Update the count of paid installments
                    };

                    // Update the existing loan and insert a new row
                    _context.Loans.Update(loan);  // Update the existing loan record
                    _context.Loans.Add(newLoanEntry);  // Add a new loan transaction
                }

                // Calculate net salary
                var grossSalary = basicSalary;
                var netSalary = grossSalary - deductionAmount;
                totalPayrollAmount += netSalary;

                // Create salary record
                var salaryRecord = new Salary
                {
                    EmployeeId = employee.EmployeeId,
                    RoleId = employee.RoleId,
                    BasicSalary = grossSalary,
                    GrossSalary = grossSalary,
                    DeductionAmount = deductionAmount,
                    DeductionType = "Loan Deduction",
                    SalaryDate = payrollDate
                };

                // Add salary record to context
                _context.Salary.Add(salaryRecord);
            }

            // Create payroll record
            var payroll = new Payroll
            {
                PayrollDate = payrollDate,
                TotalAmount = totalPayrollAmount
            };

            // Save the payroll record
            _context.Payroll.Add(payroll);

            // Save all changes (including loan updates, new loan transaction, salaries, payroll)
            await _context.SaveChangesAsync();

            return RedirectToAction("GeneratePayroll");
        }

        // GET: Admin/HRM/EmployeeList
        public async Task<IActionResult> EmployeeList()
        {
            var employees = await _context.Employee
                .Include(e => e.Role) // Include role details
                .Select(e => new EmployeeViewModel
                {
                    Id = e.EmployeeId,

                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    FatherName = e.FatherName,
                    DateOfBirth = e.DateOfBirth,
                    DateOfJoining = e.DateOfJoining,
                    Email = e.Email,
                    Phone = e.Phone,
                    EmergencyContact = e.EmergencyContact,
                    MaritalStatus = e.MaritalStatus,
                    Address = e.Address,
                    PermanentAddress = e.PermanentAddress,
                    PANNumber = e.PANNumber,
                    Qualification = e.Qualification,
                    WorkExperience = e.WorkExperience,
                    Salary = e.Salary,
                    DepartmentName = e.DepartmentName,
                    RoleName = e.Role != null ? e.Role.Name : "No Role"
                }).ToListAsync();

            return View(employees);
        }

        // GET: Admin/HRM/Edit/5
        public async Task<IActionResult> Loan()
        {
            // Retrieve all users from the User table
            var users = await _context.Employee.ToListAsync();

            // Create a SelectList for the dropdown using the correct SelectListItem
            ViewBag.Users = users.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = u.EmployeeId.ToString(), // Ensure Id is converted to string if it's a Guid
                Text = u.FirstName // Display the username
            }).ToList();

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GeneratePayroll()
        {
            // Fetch all payroll records from the database
            var payrolls = await _context.Payroll.ToListAsync();

            return View(payrolls); // Pass the payroll records to the view
        }

        public async Task<IActionResult> Employeemanagement()
        {
            var employees = await _context.Employee
                 .Include(e => e.Role) // Include role details
                 .Select(e => new EmployeeViewModel
                 {
                     Id = e.EmployeeId,

                     FirstName = e.FirstName,
                     LastName = e.LastName,
                     FatherName = e.FatherName,
                     DateOfBirth = e.DateOfBirth,
                     DateOfJoining = e.DateOfJoining,
                     Email = e.Email,
                     Phone = e.Phone,
                     EmergencyContact = e.EmergencyContact,
                     MaritalStatus = e.MaritalStatus,
                     Address = e.Address,
                     PermanentAddress = e.PermanentAddress,
                     PANNumber = e.PANNumber,
                     Qualification = e.Qualification,
                     WorkExperience = e.WorkExperience,
                     Salary = e.Salary,
                     DepartmentName = e.DepartmentName,
                     RoleName = e.Role != null ? e.Role.Name : "No Role"
                 }).ToListAsync();

            return View(employees);
        }
        public async Task<IActionResult> AddEmployee()
        {
            // Fetch all roles and convert to ViewModel
            var roles = await _context.Roles
                                        .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                                        .ToListAsync();

            var departments = await _context.Deparments
                                             .Select(d => d.Name)
                                             .ToListAsync();

            var viewModel = new AddEmployeeViewModel
            {
                Roles = roles,
                Departments = departments
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddDepartment()
        {
            return View();
        }
        public async Task<IActionResult> ManageRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee model)
        {
            // Get the selected role
            var role = await _role.FindByIdAsync(model.RoleId);

            // Create a new Employee entity
            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                FatherName = model.FatherName,
                DateOfBirth = model.DateOfBirth,
                DateOfJoining = model.DateOfJoining,
                Email = model.Email,
                Password = model.Password, // Ideally, hash the password
                Phone = model.Phone,
                EmergencyContact = model.EmergencyContact,
                MaritalStatus = model.MaritalStatus,
                Address = model.Address,
                PermanentAddress = model.PermanentAddress,
                PANNumber = model.PANNumber,
                Qualification = model.Qualification,
                WorkExperience = model.WorkExperience,
                Salary = model.Salary, // Save the salary
                DepartmentName = model.DepartmentName,
                RoleId = role.Id // Save the role ID
            };

            // Save the employee to the database
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync(); // Save employee first to generate an Employee ID



            return RedirectToAction("Employeemanagement");
        }


        public async Task<IActionResult> EmployeeAttendence()
        {
            return View();
        }
        public async Task<IActionResult> LeaveRequest()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Deparments.ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            return await _role.Roles.ToListAsync();
        }

        // GET: api/department/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(Guid id)
        {
            var department = await _context.Deparments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // POST: api/department
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(string name)
        {
            Department dep = new Department
            {
                Name = name,
                Staffs = new List<Staff>() // Initialize the required Staffs property
            };

            dep.Id = Guid.NewGuid();
            _context.Deparments.Add(dep);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartment), new { id = dep.Id }, dep);
        }

        // POST: api/department
        [HttpPost]
        public async Task<ActionResult<Department>> PostRole(string name)
        {


            var role = new IdentityRole(name);
            await _role.CreateAsync(role);

            return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, role);
        }

        // PUT: api/department/{id}
        [HttpPut]
        public async Task<IActionResult> PutDepartment(Guid id, string name)
        {
            var department = await _context.Deparments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.Name = name;
            // Ensure that the Staffs list is not null, initialize if necessary
            department.Staffs ??= new List<Staff>();

            _context.Entry(department).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/department/{id}
        [HttpPut]
        public async Task<IActionResult> PutRole(Guid id, string name)
        {
            var Role = await _role.FindByIdAsync(id.ToString());
            if (Role == null)
            {
                return NotFound();
            }

            Role.Name = name;
            // Ensure that the Staffs list is not null, initialize if necessary

            await _role.UpdateAsync(Role);

            return NoContent();
        }

        // DELETE: api/department/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var department = await _context.Deparments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Deparments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/department/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(Guid id)
        {

            var role = await _role.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }
            await _role.DeleteAsync(role);

            return NoContent();
        }

        public async Task<IActionResult> Payroll()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyLoan(LoanApplicationViewModel model)
        {

            // Create a new Loan object
            var loan = new Loan
            {
                EmployeeId = model.UserId,
                LoanAmount = model.LoanAmount,
                NumberOfInstallments = model.NumberOfInstallments,
                PerInstallmentAmount = model.PerInstallmentAmount,
                DueDate = model.DueDate,
                //PaidDate = model.PaidDate,
                PaidDate = DateTime.MinValue, // Static value for PaidDate
                RemainingAmount = model.LoanAmount,
                PaidInstallments = 0,
                RemainingInstallments = model.NumberOfInstallments,
                //status = true,
            };

            // Add loan to the database
            _context.Loan.Add(loan);
            await _context.SaveChangesAsync();

            return RedirectToAction("LoanDetails"); // Redirect to loan details page





        }
        public IActionResult GetLoansByUser(Guid userId)
        {
            var loans = _context.Loan.Where(l => l.EmployeeId == userId).ToList(); // Adjust according to your Loan model
            return View(loans); // Create a new view to display these loans
        }
        public async Task<IActionResult> LoanDetails()
        {
            var loans = await _context.Loans
                .Include(l => l.Employee) // Include Employee details
                .GroupBy(l => l.EmployeeId) // Group by EmployeeId
                .Select(g => g.OrderByDescending(l => l.DueDate).FirstOrDefault()) // Select the latest loan for each employee
                .ToListAsync();

            return View(loans); // Pass the distinct list of loans to the view
        }






    }
}

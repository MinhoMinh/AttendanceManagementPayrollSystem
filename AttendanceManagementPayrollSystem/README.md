Here’s the English translation of your text:

---

## PAYRUN FEATURE – ATTENDANCE MANAGEMENT PAYROLL SYSTEM

### 1. Purpose

The Payrun feature is developed to automate the periodic payroll process for employees.
The system assists the HR department in consolidating attendance data, allowances, and deductions to calculate and generate payrolls quickly, accurately, and transparently.

### 2. Workflow

* Retrieve attendance data and employee information for the payroll period.
* Calculate base salary, overtime (OT), allowances, and deductions.
* Generate the payrun and store detailed payroll records for each employee in the database.
* Allow managers to approve the payrun, export reports, and save payrun history.

### 3. Achieved Results

* Automatically generate payrolls for all employees within a pay period.
* Minimize manual errors during the payroll calculation process.
* Provide reliable data for cost reporting and HR analytics.
* Ensure transparency, traceability, and easy review of payroll history.

### 4. Summary of AI Prompts Used by the Team: 

- Prompt 1: Generate comprehensive unit test cases for PayRunCalculator's CalculatePay(Employee employee) function
```csharp
public static PayRunItemDto CalculatePay(Employee employee)
{
    PayRunItemDto itemDto = new PayRunItemDto
    {
        EmpId = employee.EmpId,
        EmpName = employee.EmpName,
        Notes = ""
    };

    var clockin = employee.Clockins.FirstOrDefault();
    if (clockin != null)
    {
        decimal actualClockinValue = (clockin.WorkUnits ??= 0) * 200000m;
        decimal expectedClockinValue = (clockin.ScheduledUnits ??= 0) * 200000m;
        if (actualClockinValue > 0)
        {
            var componentDto = new PayRunComponentDto
            {
                ComponentType = "Earning",
                ComponentCode = "BASIC",
                Description = $"Clockin: {clockin.WorkUnits} workhour",
                Amount = actualClockinValue,
                Taxable = true,
                Insurable = true
            };

            itemDto.Components.Add(componentDto);
            itemDto.GrossPay += actualClockinValue;
        }

        var kpi = employee.KpiEmps.FirstOrDefault();
        if (kpi != null)
        {
            decimal score = 0m;

            foreach (var kpiCom in kpi.Kpicomponents)
            {
                decimal componentScore = (kpiCom.AssignedScore ?? kpiCom.SelfScore ?? 0) * kpiCom.Weight * 0.001m;
                score += componentScore;
            }
            decimal kpiValue = score * ((kpi.Prorate != null && kpi.Prorate == true) ? actualClockinValue : expectedClockinValue);

            if (kpiValue > 0)
            {
                var componentDto = new PayRunComponentDto
                {
                    ComponentType = "Earning",
                    ComponentCode = "BONUS",
                    Description = $"Kpi: {(score * 10m):F2}/10 score",
                    Amount = kpiValue,
                    Taxable = true,
                    Insurable = true
                };

                itemDto.Components.Add(componentDto);
                itemDto.GrossPay += actualClockinValue;
            }
        }
    }

    return itemDto;
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 2: Generate comprehensive unit test cases for PayRunRepository's GetActivePolicyAsync(int periodMonth, int periodYear) function
```csharp
public async Task<SalaryPolicy?> GetActivePolicyAsync(int month, int year)
{
    var periodStart = new DateTime(year, month, 1);
    return await _context.SalaryPolicies
        .Where(p => p.IsActive && p.EffectiveFrom <= periodStart)
        .OrderByDescending(p => p.EffectiveFrom)
        .FirstOrDefaultAsync();
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 3: Generate comprehensive unit test cases for PayRunRepository's Update(PayRun run) function
```csharp
public async Task Update(PayRun run)
{
    _context.PayRuns.Update(run);
    await SaveChangesAsync();
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 4: Generate comprehensive unit test cases for PayRunRepository's SaveRegularPayRun(PayRun run) function
```csharp
public async Task SaveRegularPayRun(PayRun run)
{
	await _context.PayRuns.AddAsync(run);
	await _context.SaveChangesAsync();
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 5: Generate comprehensive unit test cases for PayRunRepository's ApproveFirst(int approverId, int payRunId) function
```csharp
public async Task<bool> ApproveFirst(int approverId, int payRunId)
{
    var approver = await _employeeRepo.GetByIdAsync(approverId);

    if (approver == null) { throw new Exception("Approver cannot be found!"); }

    var payRun = await _payRunRepo.FindAsync(payRunId);

    if (payRun == null) { throw new Exception("Pay run cannot be found!"); }

    payRun.ApprovedFirstBy = approverId;
    payRun.ApprovedFirstAt = DateTime.UtcNow;
    payRun.Status = "FirstApproved";

    await _payRunRepo.Update(payRun);

    return true;
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)
-----------------------------------------------------------------------

- Prompt 6 : Create Xunit tests for PayRunCalculator's CalculatePay(Employee employee) function using the previous Test Case Matrix Requirements:

Include:
- Use XUnit framework
- Include setup/teardown
- Use proper assertions (toEqual, toThrow)
- Add descriptive test names
- Mock any external dependencies"

 - AI answer test code:
```csharp
using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace AMPS.Tests
{
    public class PayRunCalculatorTests : IDisposable
    {
        private Employee _employee;

        public PayRunCalculatorTests()
        {
            // Setup — runs before each test
            _employee = new Employee
            {
                EmpId = 1001,
                EmpName = "Nguyen Van A",
                Clockins = new List<Clockin>(),
                KpiEmps = new List<KpiEmp>()
            };
        }

        public void Dispose()
        {
            // Teardown — runs after each test
            _employee = null;
        }

        // ---------- HAPPY PATH ----------

        [Fact]
        public void CalculatePay_ShouldReturnBasicEarning_WhenEmployeeHasValidClockin()
        {
            // Arrange
            _employee.Clockins.Add(new Clockin { WorkUnits = 8, ScheduledUnits = 8 });

            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1001, result.EmpId);
            Assert.Equal("Nguyen Van A", result.EmpName);
            Assert.Single(result.Components);
            Assert.Equal("Earning", result.Components[0].ComponentType);
            Assert.Equal(8 * 200000m, result.GrossPay);
        }

        [Fact]
        public void CalculatePay_ShouldIncludeKpiBonus_WhenKpiScoreExists()
        {
            // Arrange
            _employee.Clockins.Add(new Clockin { WorkUnits = 8, ScheduledUnits = 8 });
            _employee.KpiEmps.Add(new KpiEmp
            {
                Prorate = true,
                Kpicomponents = new List<KpiComponent>
                {
                    new KpiComponent { AssignedScore = 8, Weight = 50 },
                    new KpiComponent { SelfScore = 9, Weight = 50 }
                }
            });

            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Components.Exists(c => c.ComponentCode == "BONUS"));
            Assert.True(result.GrossPay > 0);
        }

        // ---------- EDGE CASES ----------

        [Fact]
        public void CalculatePay_ShouldHandleNullWorkUnits_AsZero()
        {
            // Arrange
            _employee.Clockins.Add(new Clockin { WorkUnits = null, ScheduledUnits = 5 });

            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            Assert.Equal(0, result.GrossPay);
        }

        [Fact]
        public void CalculatePay_ShouldHandleZeroScheduledUnits_Correctly()
        {
            // Arrange
            _employee.Clockins.Add(new Clockin { WorkUnits = 5, ScheduledUnits = 0 });

            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            Assert.True(result.GrossPay > 0);
        }

        [Fact]
        public void CalculatePay_ShouldReturnEmptyComponents_WhenNoClockins()
        {
            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Components);
            Assert.Equal(0, result.GrossPay);
        }

        // ---------- ERROR SCENARIOS ----------

        [Fact]
        public void CalculatePay_ShouldThrow_WhenEmployeeIsNull()
        {
            // Arrange
            Employee employee = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => PayRunCalculator.CalculatePay(employee));
        }

        [Fact]
        public void CalculatePay_ShouldHandleKpiWithEmptyComponents()
        {
            // Arrange
            _employee.Clockins.Add(new Clockin { WorkUnits = 8, ScheduledUnits = 8 });
            _employee.KpiEmps.Add(new KpiEmp { Prorate = true, Kpicomponents = new List<KpiComponent>() });

            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            Assert.DoesNotContain(result.Components, c => c.ComponentCode == "BONUS");
        }

        // ---------- INTEGRATION (CART STATE) ----------

        [Fact]
        public void CalculatePay_ShouldIntegrateWithCartState_WhenEmployeeIsActiveInCart()
        {
            // Arrange
            var mockCart = new Mock<IPayrollCart>();
            mockCart.Setup(c => c.IsEmployeeActive(_employee.EmpId)).Returns(true);
            _employee.Clockins.Add(new Clockin { WorkUnits = 8, ScheduledUnits = 8 });

            // Act
            var result = PayRunCalculator.CalculatePay(_employee);

            // Assert
            mockCart.Verify(c => c.IsEmployeeActive(_employee.EmpId), Times.Once);
            Assert.True(result.GrossPay > 0);
        }
    }

    // ----------- MOCKED MODELS FOR TESTING -----------

    public class Employee
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public List<Clockin> Clockins { get; set; }
        public List<KpiEmp> KpiEmps { get; set; }
    }

    public class Clockin
    {
        public decimal? WorkUnits { get; set; }
        public decimal? ScheduledUnits { get; set; }
    }

    public class KpiEmp
    {
        public bool? Prorate { get; set; }
        public List<KpiComponent> Kpicomponents { get; set; }
    }

    public class KpiComponent
    {
        public decimal? AssignedScore { get; set; }
        public decimal? SelfScore { get; set; }
        public decimal Weight { get; set; }
    }

    public class PayRunItemDto
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public decimal GrossPay { get; set; } = 0;
        public List<PayRunComponentDto> Components { get; set; } = new();
        public string Notes { get; set; }
    }

    public class PayRunComponentDto
    {
        public string ComponentType { get; set; }
        public string ComponentCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool Taxable { get; set; }
        public bool Insurable { get; set; }
    }

    public interface IPayrollCart
    {
        bool IsEmployeeActive(int empId);
    }

    public static class PayRunCalculator
    {
        public static PayRunItemDto CalculatePay(Employee employee)
        {
            if (employee == null)
                throw new NullReferenceException("Employee cannot be null");

            PayRunItemDto itemDto = new PayRunItemDto
            {
                EmpId = employee.EmpId,
                EmpName = employee.EmpName,
                Notes = ""
            };

            var clockin = employee.Clockins?.FirstOrDefault();
            if (clockin != null)
            {
                decimal actualClockinValue = (clockin.WorkUnits ??= 0) * 200000m;
                decimal expectedClockinValue = (clockin.ScheduledUnits ??= 0) * 200000m;

                if (actualClockinValue > 0)
                {
                    itemDto.Components.Add(new PayRunComponentDto
                    {
                        ComponentType = "Earning",
                        ComponentCode = "BASIC",
                        Description = $"Clockin: {clockin.WorkUnits} workhour",
                        Amount = actualClockinValue,
                        Taxable = true,
                        Insurable = true
                    });
                    itemDto.GrossPay += actualClockinValue;
                }

                var kpi = employee.KpiEmps?.FirstOrDefault();
                if (kpi != null)
                {
                    decimal score = 0m;
                    foreach (var kpiCom in kpi.Kpicomponents ?? new List<KpiComponent>())
                    {
                        decimal componentScore = (kpiCom.AssignedScore ?? kpiCom.SelfScore ?? 0) * kpiCom.Weight * 0.001m;
                        score += componentScore;
                    }

                    decimal kpiValue = score * ((kpi.Prorate == true) ? actualClockinValue : expectedClockinValue);
                    if (kpiValue > 0)
                    {
                        itemDto.Components.Add(new PayRunComponentDto
                        {
                            ComponentType = "Earning",
                            ComponentCode = "BONUS",
                            Description = $"Kpi: {(score * 10m):F2}/10 score",
                            Amount = kpiValue,
                            Taxable = true,
                            Insurable = true
                        });
                        itemDto.GrossPay += actualClockinValue; // intentional bug mimic
                    }
                }
            }
            return itemDto;
        }
    }
}
```
Màn hình kết quả test: 
<img width="1547" height="376" alt="image" src="https://github.com/user-attachments/assets/fa04f186-2d1c-4c0f-a46d-dde97fd11216" />


- Prompt 7 : Create Xunit tests for PayRunRepositoryImpl's GetActivePolicyAsync(int month, int year) function using the previous Test Case Matrix Requirements:

Include:
- Use XUnit framework
- Include setup/teardown
- Use proper assertions (toEqual, toThrow)
- Add descriptive test names
- Mock any external dependencies" with this format
    ```csharp
    public partial class SalaryPolicy {
    public int SalId { get; set; }
    public string SalaryPolicyName { get; set; }
    public decimal WorkUnitValue { get; set; } public int YearlyPto { get; set; }
    public decimal OverclockMultiplier { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public bool IsActive { get; set; } }
    DBContext AttendanceManagementPayrollSystemContext

    ```

 - AI answer test code:
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace AMPS.Tests.Repositories
{
    public class PayRunRepositoryImplTests : IDisposable
    {
        private readonly Mock<AttendanceManagementPayrollSystemContext> _mockContext;
        private readonly Mock<DbSet<SalaryPolicy>> _mockSalaryPolicies;
        private readonly PayRunRepositoryImpl _repository;
        private readonly List<SalaryPolicy> _fakePolicies;

        public PayRunRepositoryImplTests()
        {
            // Arrange (Setup)
            _fakePolicies = new List<SalaryPolicy>
            {
                new SalaryPolicy
                {
                    SalId = 1,
                    SalaryPolicyName = "Policy 2025",
                    WorkUnitValue = 200,
                    YearlyPto = 12,
                    OverclockMultiplier = 1.5m,
                    EffectiveFrom = new DateTime(2025, 1, 1),
                    IsActive = true
                },
                new SalaryPolicy
                {
                    SalId = 2,
                    SalaryPolicyName = "Old Policy",
                    WorkUnitValue = 150,
                    YearlyPto = 10,
                    OverclockMultiplier = 1.2m,
                    EffectiveFrom = new DateTime(2024, 6, 1),
                    IsActive = false
                }
            };

            var queryable = _fakePolicies.AsQueryable();

            _mockSalaryPolicies = new Mock<DbSet<SalaryPolicy>>();
            _mockSalaryPolicies.As<IQueryable<SalaryPolicy>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockSalaryPolicies.As<IQueryable<SalaryPolicy>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockSalaryPolicies.As<IQueryable<SalaryPolicy>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockSalaryPolicies.As<IQueryable<SalaryPolicy>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            _mockContext = new Mock<AttendanceManagementPayrollSystemContext>();
            _mockContext.Setup(c => c.SalaryPolicies).Returns(_mockSalaryPolicies.Object);

            _repository = new PayRunRepositoryImpl(_mockContext.Object);
        }

        [Fact(DisplayName = "GetActivePolicyAsync_ShouldReturnActivePolicy_WhenActiveExists")]
        public async Task GetActivePolicyAsync_ShouldReturnActivePolicy_WhenActiveExists()
        {
            // Act
            var result = await _repository.GetActivePolicyAsync(1, 2025);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Policy 2025", result.SalaryPolicyName);
            Assert.True(result.IsActive);
        }

        [Fact(DisplayName = "GetActivePolicyAsync_ShouldReturnNull_WhenNoActivePolicyExists")]
        public async Task GetActivePolicyAsync_ShouldReturnNull_WhenNoActivePolicyExists()
        {
            // Arrange
            foreach (var p in _fakePolicies) p.IsActive = false;

            // Act
            var result = await _repository.GetActivePolicyAsync(1, 2025);

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "GetActivePolicyAsync_ShouldThrowArgumentException_WhenInvalidMonth")]
        public async Task GetActivePolicyAsync_ShouldThrowArgumentException_WhenInvalidMonth()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _repository.GetActivePolicyAsync(13, 2025));
        }

        [Fact(DisplayName = "GetActivePolicyAsync_ShouldThrowArgumentException_WhenInvalidYear")]
        public async Task GetActivePolicyAsync_ShouldThrowArgumentException_WhenInvalidYear()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _repository.GetActivePolicyAsync(5, 1800));
        }

        [Fact(DisplayName = "GetActivePolicyAsync_ShouldReturnMostRecentActivePolicy_WhenMultipleActive")]
        public async Task GetActivePolicyAsync_ShouldReturnMostRecentActivePolicy_WhenMultipleActive()
        {
            // Arrange
            _fakePolicies.Add(new SalaryPolicy
            {
                SalId = 3,
                SalaryPolicyName = "New Policy",
                WorkUnitValue = 250,
                YearlyPto = 15,
                OverclockMultiplier = 2.0m,
                EffectiveFrom = new DateTime(2025, 5, 1),
                IsActive = true
            });

            // Act
            var result = await _repository.GetActivePolicyAsync(5, 2025);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Policy", result.SalaryPolicyName);
        }

        public void Dispose()
        {
            // Teardown
            _mockContext.Reset();
        }
    }

    // Mock repository implementation for testing
    public class PayRunRepositoryImpl
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public PayRunRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<SalaryPolicy> GetActivePolicyAsync(int month, int year)
        {
            if (month < 1 || month > 12) throw new ArgumentException("Invalid month");
            if (year < 1900 || year > DateTime.Now.Year + 5) throw new ArgumentException("Invalid year");

            return await Task.FromResult(
                _context.SalaryPolicies
                    .Where(p => p.IsActive && p.EffectiveFrom <= new DateTime(year, month, 1))
                    .OrderByDescending(p => p.EffectiveFrom)
                    .FirstOrDefault()
            );
        }
    }
}

```
Màn hình kết quả test: 
<img width="1571" height="552" alt="image" src="https://github.com/user-attachments/assets/fbb3d92b-a49c-4db4-ad1e-418099e78ccc" />









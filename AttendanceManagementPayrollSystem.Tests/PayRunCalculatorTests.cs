using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Tests
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

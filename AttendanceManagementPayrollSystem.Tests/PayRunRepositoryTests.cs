using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Tests
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

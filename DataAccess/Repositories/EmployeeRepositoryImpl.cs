using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Kpi = AttendanceManagementPayrollSystem.Models.Kpi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeRepositoryImpl : EmployeeRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public EmployeeRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        // ✅ Add new employee
        public async Task<Employee> AddAsync(Employee emp)
        {
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();
            return emp;
        }

        // ✅ Get all employees
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByDepartment(int depId, params Expression<Func<Employee, object>>[] includes)
        {
            IQueryable<Employee> query = _context.Employees.Where(e => e.DepId == depId);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentWithKPI(int depId)
        {
            return await _context.Employees
                .Where(e => e.DepId == depId)
                .Include(e => e.KpiEmps)
                    .ThenInclude(kpi => kpi.Kpicomponents).ToListAsync();

            //return await query.ToListAsync();
        }

        // ✅ Get by ID
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // ✅ Update employee
        public async Task<Employee?> UpdateAsync(Employee emp)
        {
            // 1. Find existing entity in the database
            var existing = await _context.Employees.FindAsync(emp.EmpId);
            if (existing == null)
            {
                // Employee not found
                return null;
            }

            // 2. Update properties
            existing.EmpName = emp.EmpName;
            // add other fields as needed

            // 3. Save changes
            await _context.SaveChangesAsync();

            // 4. Return updated entity
            return existing;
        }

        // ✅ Find by username (for login)
        public async Task<Employee?> FindByUsernameAsync(string username)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Username == username);
        }

        public async Task LoadRoles(Employee employee)
        {
            await _context.Entry(employee)
                .Collection(e => e.EmployeeRoles)
                .Query()
                .Include(er => er.Role)
                    //.ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permissions)
                .LoadAsync();
        }
    }
}

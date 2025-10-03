using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeRepositoryImpl : EmployeeRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public EmployeeRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }


        public async Task<Employee> AddAsync(Employee emp)
        {
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();
            return emp;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

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
    }
}

using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

public class OvertimeRepositoryImpl : IOvertimeRepository
{
    private readonly AttendanceManagementPayrollSystemContext _context;

    public OvertimeRepositoryImpl(AttendanceManagementPayrollSystemContext context)
    {
        _context = context;
    }

    public IEnumerable<OvertimeRequest> GetOvertimeByEmployeeId(int empId)
    {
        return _context.OvertimeRequests
            .Include(o => o.OvertimeTypeNavigation)
            .Include(o => o.ApprovedByNavigation)
            .Where(o => o.EmpId == empId)
            .OrderByDescending(o => o.ReqDate)
            .ToList();
    }

    public OvertimeRequest GetById(int id)
    {
        return _context.OvertimeRequests
            .Include(o => o.OvertimeTypeNavigation)
            .Include(o => o.ApprovedByNavigation)
            .FirstOrDefault(o => o.ReqId == id);
    }

    public void Create(OvertimeRequest request)
    {
        _context.OvertimeRequests.Add(request);
        _context.SaveChanges();
    }

    public void Approve(int reqId, int approverId)
    {
        var req = _context.OvertimeRequests.Find(reqId);
        if (req == null) return;

        req.Status = "Approved";
        req.ApprovedBy = approverId;
        req.ApprovedDate = DateOnly.FromDateTime(DateTime.Now);
        _context.SaveChanges();
    }

    public void Reject(int reqId, int approverId)
    {
        var req = _context.OvertimeRequests.Find(reqId);
        if (req == null) return;

        req.Status = "Rejected";
        req.ApprovedBy = approverId;
        req.ApprovedDate = DateOnly.FromDateTime(DateTime.Now);
        _context.SaveChanges();
    }
}

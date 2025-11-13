using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

public class OvertimeRepositoryImpl : IOvertimeRepository
{
    private readonly AttendanceManagementPayrollSystemContext _context;

    public OvertimeRepositoryImpl(AttendanceManagementPayrollSystemContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OvertimeRequest>> GetOvertimeByEmployeeId(
    int empId,
    DateOnly? startDate,
    DateOnly? endDate)
    {
        var query = _context.OvertimeRequests
            .Include(o => o.OvertimeTypeNavigation)
            .Include(o => o.ApprovedByNavigation)
            .Include(o => o.LinkedPayrollRun)
            .Where(o => o.EmpId == empId);

        if (startDate.HasValue)
            query = query.Where(o => o.ReqDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(o => o.ReqDate <= endDate.Value);

        var list = await query
            .OrderByDescending(o => o.ReqDate)
            .ToListAsync();

        Console.WriteLine($"count {list.Count}");
        return list;
    }

    public async Task<Dictionary<int, List<OvertimeRequest>>> GetApprovedOvertimes(
    DateOnly startDate,
    DateOnly endDate)
    {
        var list = await _context.OvertimeRequests
            .Include(o => o.OvertimeTypeNavigation)
            .Where(o => o.ReqDate >= startDate)
            .Where(o => o.ReqDate <= endDate)
            .Where(o => o.Status == "Approved")
            .OrderByDescending(o => o.ReqDate)
            .ToListAsync();

        //Console.WriteLine($"count {list.Count}");

        return list
            .GroupBy(o => o.EmpId)
            .ToDictionary(g => g.Key, g => g.ToList());
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

    public IEnumerable<OvertimeRate> GetRates()
    {
        return _context.OvertimeRates;
    }
}

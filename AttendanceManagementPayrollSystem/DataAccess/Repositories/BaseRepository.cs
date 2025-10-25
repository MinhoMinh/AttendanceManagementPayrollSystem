namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface BaseRepository
    {
        Task<int> SaveChangesAsync();
    }
}

namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public interface TimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class SystemTimeProvider : TimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }

    public class FixedTimeProvider : TimeProvider
    {
        public DateTime UtcNow { get; }
        public FixedTimeProvider(DateTime fixedTime) => UtcNow = fixedTime;
    }
}

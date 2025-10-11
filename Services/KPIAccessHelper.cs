namespace AttendanceManagementPayrollSystem.Services
{
    public class KPIAccessHelper
    {
        public static string GetKpiMode(int month, int year, string role)
        {
            var today = DateTime.UtcNow.Date;
            var kpiMonth = new DateTime(year, month, 1);

            var startSelf = kpiMonth.AddMonths(1);           // 1st of next month
            var endSelf = startSelf.AddDays(3);              // 3 days for self-scoring
            var endAssign = endSelf.AddDays(3);              // 3 days for assigning

            if (role == "employee")
            {
                if (today >= startSelf && today < endSelf)
                    return "self";
                return "view";
            }

            if (role == "manager")
            {
                if (today >= endSelf && today < endAssign)
                    return "assign";
                return "view";
            }

            return "view";
        }

    }
}

namespace AttendanceManagementPayrollSystem.Services
{
    public class KPIAccessHelper
    {
        public static string GetKpiMode(int month, int year, string role)
        {
            var today = DateTime.UtcNow.Date;
            var kpiMonth = new DateTime(year, month, 1);

            var startSelf = kpiMonth.AddMonths(1);           // 1st of next month
            var endSelf = startSelf.AddDays(2);              // 3 days for self-scoring
            var endAssign = endSelf.AddDays(3);              // 3 days for assigning
            var endEdit = kpiMonth.AddDays(-1);
            var startEdit = endEdit.AddDays(-2);


            if (role == "employee")
            {
                if (today >= startSelf && today < endSelf)
                    return "self";
                return "view";
                //return "self";
            }

            if (role == "head")
            {
                if (today >= endSelf && today < endAssign)
                    return "assign";
                else if (today >= startEdit && today <= endEdit)
                    return "edit";
                return "view";
                //return "assign";
            }

            if (role == "manager")
            {
                return "view";
            }

            return "view";
        }

    }
}

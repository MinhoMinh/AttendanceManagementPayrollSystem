namespace AttendanceManagementPayrollSystem.Services.Helper
{

    public static class KpiAccessHelper
    {
        public static string GetKpiMode(int month, int year, string role, TimeProvider? timeProvider = null)
        {
            var provider = timeProvider ?? new SystemTimeProvider();
            var today = provider.UtcNow.Date;
            var kpiMonth = new DateTime(year, month, 1);

            Console.WriteLine(today);
            Console.WriteLine(kpiMonth);


            var startSelf = kpiMonth.AddMonths(1);   
            var endSelf = startSelf.AddDays(2);     
            var endAssign = endSelf.AddDays(3);      
            var startEdit = kpiMonth.AddDays(-3);    
            var endEdit = kpiMonth.AddDays(-1);

            Console.WriteLine(startEdit);

            return role switch
            {
                "employee" when today >= startSelf && today < endSelf => "self",
                "employee" => "view",

                "head" when today >= endSelf && today < endAssign => "assign",
                "head" when today >= startEdit && today <= endEdit => "edit",
                "head" => "view",

                "manager" => "view",
                _ => "view"
            };
        }
    }

}

PAYRUN FEATURE – ATTENDANCE MANAGEMENT PAYROLL SYSTEM

1. Mục đích
Tính năng Payrun được xây dựng nhằm tự động hóa quá trình chạy bảng lương định kỳ cho nhân viên.
Hệ thống giúp bộ phận nhân sự tổng hợp dữ liệu chấm công, phụ cấp và khấu trừ để tính toán và tạo bảng lương một cách nhanh chóng, chính xác và minh bạch.

2. Luồng hoạt động
- Lấy dữ liệu chấm công và thông tin nhân viên trong kỳ tính lương.
- Tính toán lương cơ bản, làm thêm giờ (OT), phụ cấp và các khoản khấu trừ.
- Tạo bảng lương (Payrun) và lưu chi tiết từng nhân viên trong cơ sở dữ liệu.
- Cho phép người quản lý phê duyệt bảng lương, xuất báo cáo và lưu lịch sử payrun.

3. Kết quả đạt được
- Tự động sinh bảng lương cho toàn bộ nhân viên trong kỳ.
- Giảm thiểu sai sót thủ công trong quá trình tính toán lương.
- Cung cấp dữ liệu đáng tin cậy cho báo cáo chi phí và phân tích nhân sự.
- Đảm bảo tính minh bạch, có thể truy vết và kiểm tra lịch sử trả lương.

4. Tổng hợp các prompt AI nhóm đã sử dụng:

- Prompt 1: Generate comprehensive unit test cases for PayRunCalculator's CalculatePay(Employee employee) function
```csharp
public static PayRunItemDto CalculatePay(Employee employee)
{
    PayRunItemDto itemDto = new PayRunItemDto
    {
        EmpId = employee.EmpId,
        EmpName = employee.EmpName,
        Notes = ""
    };

    var clockin = employee.Clockins.FirstOrDefault();
    if (clockin != null)
    {
        decimal actualClockinValue = (clockin.WorkUnits ??= 0) * 200000m;
        decimal expectedClockinValue = (clockin.ScheduledUnits ??= 0) * 200000m;
        if (actualClockinValue > 0)
        {
            var componentDto = new PayRunComponentDto
            {
                ComponentType = "Earning",
                ComponentCode = "BASIC",
                Description = $"Clockin: {clockin.WorkUnits} workhour",
                Amount = actualClockinValue,
                Taxable = true,
                Insurable = true
            };

            itemDto.Components.Add(componentDto);
            itemDto.GrossPay += actualClockinValue;
        }

        var kpi = employee.KpiEmps.FirstOrDefault();
        if (kpi != null)
        {
            decimal score = 0m;

            foreach (var kpiCom in kpi.Kpicomponents)
            {
                decimal componentScore = (kpiCom.AssignedScore ?? kpiCom.SelfScore ?? 0) * kpiCom.Weight * 0.001m;
                score += componentScore;
            }
            decimal kpiValue = score * ((kpi.Prorate != null && kpi.Prorate == true) ? actualClockinValue : expectedClockinValue);

            if (kpiValue > 0)
            {
                var componentDto = new PayRunComponentDto
                {
                    ComponentType = "Earning",
                    ComponentCode = "BONUS",
                    Description = $"Kpi: {(score * 10m):F2}/10 score",
                    Amount = kpiValue,
                    Taxable = true,
                    Insurable = true
                };

                itemDto.Components.Add(componentDto);
                itemDto.GrossPay += actualClockinValue;
            }
        }
    }

    return itemDto;
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 2: Generate comprehensive unit test cases for PayRunRepository's GetActivePolicyAsync(int periodMonth, int periodYear) function
```csharp
public async Task<SalaryPolicy?> GetActivePolicyAsync(int month, int year)
{
    var periodStart = new DateTime(year, month, 1);
    return await _context.SalaryPolicies
        .Where(p => p.IsActive && p.EffectiveFrom <= periodStart)
        .OrderByDescending(p => p.EffectiveFrom)
        .FirstOrDefaultAsync();
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 3: Generate comprehensive unit test cases for PayRunRepository's Update(PayRun run) function
```csharp
public async Task Update(PayRun run)
{
    _context.PayRuns.Update(run);
    await SaveChangesAsync();
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 4: Generate comprehensive unit test cases for PayRunRepository's SaveRegularPayRun(PayRun run) function
```csharp
public async Task SaveRegularPayRun(PayRun run)
{
	await _context.PayRuns.AddAsync(run);
	await _context.SaveChangesAsync();
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)

-----------------------------------------------------------------------

- Prompt 5: Generate comprehensive unit test cases for PayRunRepository's ApproveFirst(int approverId, int payRunId) function
```csharp
public async Task<bool> ApproveFirst(int approverId, int payRunId)
{
    var approver = await _employeeRepo.GetByIdAsync(approverId);

    if (approver == null) { throw new Exception("Approver cannot be found!"); }

    var payRun = await _payRunRepo.FindAsync(payRunId);

    if (payRun == null) { throw new Exception("Pay run cannot be found!"); }

    payRun.ApprovedFirstBy = approverId;
    payRun.ApprovedFirstAt = DateTime.UtcNow;
    payRun.Status = "FirstApproved";

    await _payRunRepo.Update(payRun);

    return true;
}
```
Include:
- Happy path scenarios
- Edge cases (boundary values)
- Error scenarios
- Integration with cart state
Output In Form of Test Cases Matrix(Category, Test Case, Input, Expected)
-----------------------------------------------------------------------

- Prompt 6 : Create Xunit tests for PayRunCalculator's CalculatePay(Employee employee) function using the previous Test Case Matrix Requirements:

Include:
- Use XUnit framework
- Include setup/teardown
- Use proper assertions (toEqual, toThrow)
- Add descriptive test names
- Mock any external dependencies"






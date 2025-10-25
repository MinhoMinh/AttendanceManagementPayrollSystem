# PAYRUN FEATURE – ATTENDANCE MANAGEMENT PAYROLL SYSTEM

## 1. Mục đích
Tính năng **Payrun** được xây dựng nhằm tự động hóa quá trình chạy bảng lương định kỳ cho nhân viên.  
Hệ thống giúp bộ phận nhân sự tổng hợp dữ liệu chấm công, phụ cấp và khấu trừ để tính toán và tạo bảng lương một cách nhanh chóng, chính xác và minh bạch.

---

## 2. Luồng hoạt động
1. Lấy dữ liệu chấm công và thông tin nhân viên trong kỳ tính lương.  
2. Tính toán lương cơ bản, làm thêm giờ (OT), phụ cấp và các khoản khấu trừ.  
3. Tạo bảng lương (Payrun) và lưu chi tiết từng nhân viên trong cơ sở dữ liệu.  
4. Cho phép người quản lý phê duyệt bảng lương, xuất báo cáo và lưu lịch sử payrun.

---

## 3. Kết quả đạt được
- Tự động sinh bảng lương cho toàn bộ nhân viên trong kỳ.  
- Giảm thiểu sai sót thủ công trong quá trình tính toán lương.  
- Cung cấp dữ liệu đáng tin cậy cho báo cáo chi phí và phân tích nhân sự.  
- Đảm bảo tính minh bạch, có thể truy vết và kiểm tra lịch sử trả lương.

---

## 4. Tổng hợp các prompt AI nhóm đã sử dụng

### Generate comprehensive unit test cases for PayRunCalculator's CalculatePay(Employee employee) function
[Code omitted for brevity — includes `PayRunCalculator.CalculatePay(Employee employee)`]

**Include:**
- Happy path scenarios  
- Edge cases (boundary values)  
- Error scenarios  
- Integration with cart state  
**Output:** In form of Test Cases Matrix (Category, Test Case, Input, Expected)

---

### Generate comprehensive unit test cases for PayRunRepository's GetActivePolicyAsync(int periodMonth, int periodYear) function
[Code for `GetActivePolicyAsync` included]

**Include:**
- Happy path scenarios  
- Edge cases (boundary values)  
- Error scenarios  
- Integration with cart state  
**Output:** In form of Test Cases Matrix (Category, Test Case, Input, Expected)

---

### Generate comprehensive unit test cases for PayRunRepository's Update(PayRun run) function
[Code for `Update` method included]

**Include:**
- Happy path scenarios  
- Edge cases (boundary values)  
- Error scenarios  
- Integration with cart state  
**Output:** In form of Test Cases Matrix (Category, Test Case, Input, Expected)

---

### Generate comprehensive unit test cases for PayRunRepository's SaveRegularPayRun(PayRun run) function
[Code for `SaveRegularPayRun` method included]

**Include:**
- Happy path scenarios  
- Edge cases (boundary values)  
- Error scenarios  
- Integration with cart state  
**Output:** In form of Test Cases Matrix (Category, Test Case, Input, Expected)

---

### Generate comprehensive unit test cases for PayRunRepository's ApproveFirst(int approverId, int payRunId) function
[Code for `ApproveFirst` method included]

**Include:**
- Happy path scenarios  
- Edge cases (boundary values)  
- Error scenarios  
- Integration with cart state  
**Output:** In form of Test Cases Matrix (Category, Test Case, Input, Expected)

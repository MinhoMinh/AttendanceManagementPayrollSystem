import { useState } from "react";
import "../components/LeaveRequest.css";

export default function LeaveRequest({ onBack }) {
  const [startDate, setStartDate] = useState(""); // ngày bắt đầu nghỉ
  const [endDate, setEndDate] = useState("");     // ngày kết thúc nghỉ
  const [reason, setReason] = useState("");       // lý do
  const [otherReason, setOtherReason] = useState(""); // lý do khác
  const [details, setDetails] = useState("");     // chi tiết lý do
  const [leaveType, setLeaveType] = useState(""); // loại nghỉ (có phép/không phép)
  const [history, setHistory] = useState([]);
  const [showHistory, setShowHistory] = useState(false);
  const [empId] = useState(3); // ID nhân viên tạm thời

  const handleSubmit = async (e) => {
  e.preventDefault();

  const typeMap = {
    co_phep: 1,         // Annual Leave
    thai_san: 2,        // Maternity Leave
    nghi_vo_sinh: 3,    // Paternity Leave
    nghi_khong_luong: 4 // Unpaid Leave
  };

  const leaveData = {
    empId: 3,
    startDate,
    endDate,
    reason: reason === "khac" ? otherReason : reason,
    details,
    typeId: typeMap[leaveType],
  };

  try {
    const response = await fetch("https://localhost:7184/api/LeaveRequest", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(leaveData),
    });

    if (response.ok) {
      alert("✅ Gửi đơn nghỉ thành công!");
      onBack();
    } else {
      alert("❌ Lỗi khi gửi đơn nghỉ!");
    }
  } catch (error) {
    console.error("Error:", error);
    alert("⚠️ Không thể kết nối đến máy chủ!");
  }
};
  const fetchHistory = async () => {
    try {
      const res = await fetch(`https://localhost:7184/api/LeaveRequest/history/${empId}`);
      if (!res.ok) throw new Error("Không lấy được dữ liệu lịch sử!");
      const data = await res.json();
      setHistory(Array.isArray(data) ? data : []);
      setShowHistory(true);
    } catch (err) {
      alert("⚠️ Không thể tải lịch sử nghỉ phép!");
      console.error(err);
    }
  };


  return (
    <div className="leave-wrapper">
      <div className="leave-card">
        {/* Nút quay lại */}
        <button onClick={onBack} className="back-btn">
          ⬅ Quay lại
        </button>

        <h2>📄 Đơn xin nghỉ</h2>
        <form onSubmit={handleSubmit}>
          {/* Ngày bắt đầu */}
          <label>
            Ngày bắt đầu:
            <input
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              required
            />
          </label>

          {/* Ngày kết thúc */}
          <label>
            Ngày kết thúc:
            <input
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              required
            />
          </label>

          {/* Chọn loại nghỉ */}
          <label>
            Loại nghỉ:
            <select
              value={leaveType}
              onChange={(e) => setLeaveType(e.target.value)}
              required
            >
              <option value="">-- Chọn loại nghỉ --</option>
              <option value="co_phep">Nghỉ có phép</option>
              <option value="thai_san">Nghỉ thai sản</option>
              <option value="nghi_vo_sinh">Nghỉ vợ sinh</option>
              <option value="nghi_khong_luong">Nghỉ không phép</option>
            </select>
          </label>

          {/* Chọn lý do nghỉ */}
          <label>
            Lý do nghỉ:
            <select
              value={reason}
              onChange={(e) => setReason(e.target.value)}
              required
            >
              <option value="">-- Chọn lý do --</option>
              <option value="om">Ốm</option>
              <option value="viec_gia_dinh">Việc gia đình</option>
              <option value="khac">Khác</option>
            </select>
          </label>

          {/* Nếu chọn "khác" thì hiện input */}
          {reason === "khac" && (
            <input
              type="text"
              placeholder="Nhập lý do khác..."
              value={otherReason}
              onChange={(e) => setOtherReason(e.target.value)}
              required
            />
          )}

          {/* Nhập chi tiết lý do */}
          <label>
            Lý do chi tiết:
            <textarea
              placeholder="Nhập chi tiết lý do nghỉ..."
              value={details}
              onChange={(e) => setDetails(e.target.value)}
              rows={4}
              required
            />
          </label>

          {/* Nút gửi */}
          <button type="submit">Gửi đơn</button>
        </form>

        {/* Nút xem lịch sử */}
        <button onClick={fetchHistory} className="history-btn">
          📋 Xem lịch sử nghỉ phép
        </button>

        {/* Bảng lịch sử nghỉ phép */}
        {showHistory && (
          <div className="history-section">
            <h3>🕓 Lịch sử đơn nghỉ</h3>
            <table className="table-history">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Bắt đầu</th>
                  <th>Kết thúc</th>
                  <th>Lý do</th>
                  <th>Loại</th>
                  <th>Trạng thái</th>
                </tr>
              </thead>
              <tbody>
                {history.length === 0 ? (
                  <tr><td colSpan="6">Không có đơn nào</td></tr>
                ) : (
                  history.map((item) => (
                    <tr key={item.reqId}>
                      <td>{item.reqId}</td>
                      <td>{item.startDate.slice(0, 10)}</td>
                      <td>{item.endDate.slice(0, 10)}</td>
                      <td>{item.reason}</td>
                      <td>{item.typeId}</td>
                      <td>{item.status}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}

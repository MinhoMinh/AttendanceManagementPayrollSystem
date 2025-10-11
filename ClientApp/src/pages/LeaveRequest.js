import { useState } from "react";
import "../components/LeaveRequest.css";

export default function LeaveRequest({ onBack }) {
  const [startDate, setStartDate] = useState(""); // ngày bắt đầu nghỉ
  const [endDate, setEndDate] = useState("");     // ngày kết thúc nghỉ
  const [reason, setReason] = useState("");       // lý do
  const [otherReason, setOtherReason] = useState(""); // lý do khác
  const [details, setDetails] = useState("");     // chi tiết lý do
  const [leaveType, setLeaveType] = useState(""); // loại nghỉ (có phép/không phép)

  const handleSubmit = (e) => {
    e.preventDefault();
    alert(
      `📅 Nghỉ từ: ${startDate} ➝ ${endDate}
📝 Loại nghỉ: ${leaveType}
📩 Lý do: ${reason === "khac" ? otherReason : reason}
📌 Chi tiết: ${details}`
    );
    onBack();
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
              <option value="khong_phep">Nghỉ không phép</option>
              <option value="nghi_bu">Nghỉ bù</option>
              <option value="thai_san">Nghỉ thai sản</option>
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
      </div>
    </div>
  );
}

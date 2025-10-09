import { useState } from "react";
import CustomButton from "../components/CustomButton";
import attendanceIcon from "../assets/icons/attendance.png";
import leavequestIcon from "../assets/icons/leaverequest.png";
import payrollIcon from "../assets/icons/payroll.png";
import reportsIcon from "../assets/icons/reports.png";
import settingsIcon from "../assets/icons/settings.png";
import logoutIcon from "../assets/icons/logout.png";


function Dashboard({ onLeaveRequest, onLogout }) {
  const [employeeName] = useState("Nguyen Van A");

  return (
    <div style={{ fontFamily: "Arial, sans-serif", minHeight: "100vh", backgroundColor: "#f4f6f9" }}>
      {/* Header */}
      <div
        style={{
          backgroundColor: "#2c3e50",
          color: "white",
          padding: "15px 30px",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          boxShadow: "0 4px 6px rgba(0,0,0,0.1)"
        }}
      >
        <h2 style={{ margin: 0 }}>📊 Dashboard</h2>
        <div style={{ fontSize: "16px", fontWeight: "bold" }}>👤 {employeeName}</div>
      </div>

      {/* Main content */}
      <div
        style={{
          padding: "40px",
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(150px, 1fr))",
          gap: "25px"
        }}
      >
        {/* Điểm danh */}
        <CustomButton
          image={attendanceIcon}
          label="Attendance"
          bgColor="#27ae60"
          size={140}
          iconSize={70}
        />

        {/* Nút xin nghỉ phép */}
        <CustomButton
          image={leavequestIcon}
          label="Leave Request"
          bgColor="#27ae60"
          size={140}
          iconSize={70}
          onClick={onLeaveRequest}
        />

        <CustomButton
          image={payrollIcon}
          label="Payroll"
          bgColor="#27ae60"
          size={140}
          iconSize={70}
        />
        <CustomButton
          image={reportsIcon}
          label="Reports"
          bgColor="#27ae60"
          size={140}
          iconSize={70}
        />
        <CustomButton
          image={settingsIcon}
          label="Settings"
          bgColor="#27ae60"
          size={140}
          iconSize={70}
        />

        {/* Nút logout */}
        <CustomButton
          image={logoutIcon}
          label="Logout"
          bgColor="#c0392b"
          size={140}
          iconSize={70}
          onClick={onLogout}
        />
      </div>
    </div>
  );
}

export default Dashboard;

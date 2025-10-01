import { useState } from "react";
import CustomButton from "./CustomButton";

function Dashboard() {
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
      <div style={{ padding: "40px", display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(150px, 1fr))", gap: "25px" }}>
        <CustomButton image="./icons/attendance.png" label="Attendance" bgColor="#27ae60" size={140} iconSize={70} />
        <CustomButton image="/icons/payroll.png" label="Payroll" bgColor="#2980b9" size={140} iconSize={70} />
        <CustomButton image="/icons/reports.png" label="Reports" bgColor="#8e44ad" size={140} iconSize={70} />
        <CustomButton image="/icons/settings.png" label="Settings" bgColor="#f39c12" size={140} iconSize={70} />
        <CustomButton image="/icons/logout.png" label="Logout" bgColor="#e74c3c" size={140} iconSize={70} />
      </div>
    </div>
  );
}

export default Dashboard;

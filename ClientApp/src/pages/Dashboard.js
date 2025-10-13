import { useState } from "react";
import { useNavigate } from "react-router-dom";
import CustomButton from "../components/CustomButton";
import attendanceIcon from "../assets/icons/attendance.png";
import leavequestIcon from "../assets/icons/leaverequest.png";
import payrollIcon from "../assets/icons/payroll.png";
import reportsIcon from "../assets/icons/reports.png";
import settingsIcon from "../assets/icons/settings.png";
import logoutIcon from "../assets/icons/logout.png";

function Dashboard() {
    const [employeeName] = useState("Nguyen Van A");

    const emp = JSON.parse(localStorage.getItem("employee"));
    const hasPermission = (perm) => emp?.permissions?.includes(perm);

    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem("employee");
        navigate("/login");
    };

    return (
        <div
            style={{
                fontFamily: "Arial, sans-serif",
                minHeight: "100vh",
                backgroundColor: "#f4f6f9"
            }}
        >
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
                {hasPermission("has_clockin") && (
                <CustomButton
                    image={attendanceIcon}
                    label="Attendance"
                    onClick={() => navigate("/attendance")}
                    />)}

                {hasPermission("has_leave_request") && (
                <CustomButton
                    image={leavequestIcon}
                    label="Leave Request"
                    onClick={() => navigate("/leave-request")}
                    />)}

                {hasPermission("generate_payroll_run") && (
                    <CustomButton
                        image={leavequestIcon}
                        label="Generate Payroll"
                        onClick={() => navigate("/generate-payroll")}
                    />)}

                {hasPermission("has_kpi") && (
                    <CustomButton
                        image={leavequestIcon}
                        label="Self KPI"
                        onClick={() => navigate("/self-kpi")}
                    />)}

                <CustomButton
                    image={payrollIcon}
                    label="Payroll"
                    onClick={() => navigate("/payroll")}
                />

                <CustomButton
                    image={reportsIcon}
                    label="Reports"
                    onClick={() => navigate("/reports")}
                />

                <CustomButton
                    image={settingsIcon}
                    label="Settings"
                    onClick={() => navigate("/settings")}
                />

                <CustomButton
                    image={logoutIcon}
                    label="Logout"
                    onClick={handleLogout}
                />
            </div>
        </div>
    );
}

export default Dashboard;

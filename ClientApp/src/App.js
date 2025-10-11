//import React, { useState } from "react";
//import Header from "./components/Header";
//import CheckAttendance from "./components/Calendar";
//import ButtonDemo from "./components/ButtonDemo";
//import Dashboard from "./pages/Dashboard";
//import ViewAdjustmentRequests from "./components/ViewAdjustmentRequests";
//import "./App.css";
//import CustomButton from "./components/CustomButton";

//function App() {
//  const [currentView, setCurrentView] = useState("dashboard"); // 'dashboard', 'attendance', 'buttons', 'adjustments'

//  const handleLogout = () => {
//    alert("Đăng xuất thành công!");
//    // Thêm logic đăng xuất ở đây
//  };

//  return (
//    <>
//    <Dashboard/>
//      <h1>Babaji</h1>
//    </>
//  );
//}

//export default App;


import React from "react";
import GeneratePayroll from "./pages/GeneratePayroll";
import ApprovePayroll from "./pages/ApprovePayroll";
import TestKpiPage from "./pages/KpiPageBase";
import EmployeeKpiPage from "./pages/EmployeeKpiPage";

//function App() {
//    const [message, setMessage] = useState("Loading...");

//    useEffect(() => {
//        fetch("http://localhost:5038/api/employees")
//            .then(response => response.json())
//            .then(data => setMessage(data.message))
//            .catch(err => console.error("Fetch error:", err));
//    }, []);

//    return (
//        <div>
//            <h1>{message}</h1>
//        </div>
//    );
//}

function App() {
    return (
        <div>
            <h1>Payroll Management</h1>
            <EmployeeKpiPage />
        </div>
    );
}

export default App;


import React, { useState } from "react";
import Header from "./components/Header";
import CheckAttendance from "./components/Calendar";
import ButtonDemo from "./components/ButtonDemo";
import Dashboard from "./pages/Dashboard";
import ViewAdjustmentRequests from "./components/ViewAdjustmentRequests";
import "./App.css";
import CustomButton from "./components/CustomButton";

import Login from "./pages/Login";
import Welcome from "./pages/Welcome";
import LeaveRequest from "./pages/LeaveRequest";

function App() {
  // State điều hướng view
  const [currentView, setCurrentView] = useState("login"); // login, welcome, dashboard, leave
  const [username, setUsername] = useState("");

  const handleLogout = () => {
    alert("Đăng xuất thành công!");
    setCurrentView("login"); // quay về login
  };

  return (
    <>
      {currentView === "login" && (
        <Login
          onLogin={(uname) => {
            setUsername(uname);
            setCurrentView("welcome");
          }}
        />
      )}

      {currentView === "welcome" && (
        <Welcome
          username={username}
          onContinue={() => setCurrentView("dashboard")}
        />
      )}

      {currentView === "dashboard" && (
        <Dashboard
          onLeaveRequest={() => setCurrentView("leave")}
          onLogout={handleLogout}
        />
      )}

      {currentView === "leave" && (
        <LeaveRequest onBack={() => setCurrentView("dashboard")} />
      )}
    </>
  );
}

export default App;


import React from "react";
import GeneratePayroll from "./pages/GeneratePayroll";
import ApprovePayroll from "./pages/ApprovePayroll";
import TestKpiPage from "./pages/KpiPageBase";
import EmployeeKpiPage from "./pages/EmployeeKpiPage";

// import React from "react";
// import GeneratePayroll from "./pages/GeneratePayroll";

// function App() {
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
// }

function App() {
    return (
        <div>
            <h1>Payroll Management</h1>
            <EmployeeKpiPage />
        </div>
    );
}

// export default App;


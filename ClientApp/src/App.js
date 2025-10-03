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


import React, { useEffect, useState } from "react";

function App() {
    const [message, setMessage] = useState("Loading...");

    useEffect(() => {
        fetch("http://localhost:5038/api/employees")
            .then(response => response.json())
            .then(data => setMessage(data.message))
            .catch(err => console.error("Fetch error:", err));
    }, []);

    return (
        <div>
            <h1>{message}</h1>
        </div>
    );
}

export default App;


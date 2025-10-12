import React from "react";
import { BrowserRouter as Router, Routes, Route, useNavigate } from "react-router-dom";

import Header from "./components/Header";
import CheckAttendance from "./components/Calendar";
import ButtonDemo from "./components/ButtonDemo";
import Dashboard from "./pages/Dashboard";
import ViewAdjustmentRequests from "./components/ViewAdjustmentRequests";
import CustomButton from "./components/CustomButton";

import Login from "./pages/Login";
import Welcome from "./pages/Welcome";
import LeaveRequest from "./pages/LeaveRequest";
import { Navigate } from "react-router-dom";

import "./App.css";
import GeneratePayroll from "./pages/GeneratePayroll";

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginWrapper />} />
                <Route path="/login" element={<LoginWrapper />} />

                <Route
                    path="/dashboard"
                    element={
                        <ProtectedRoute>
                            <Dashboard />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/leave-request"
                    element={
                        <ProtectedRoute>
                            <LeaveRequest />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/generate-payroll"
                    element={
                        <ProtectedRoute>
                            <GeneratePayrollWrapper />
                        </ProtectedRoute>
                    }
                />
            </Routes>
        </Router>
    );
}

// Wrappers handle navigation logic

function ProtectedRoute({ children }) {
    const emp = localStorage.getItem("employee");
    if (!emp) {
        return <Navigate to="/login" replace />;
    }
    return children;
}

function LoginWrapper() {
    const navigate = useNavigate();
    return (
        <Login
            onLogin={(uname) => {
                localStorage.setItem("username", uname);
                navigate("/dashboard");
            }}
        />
    );
}

function GeneratePayrollWrapper() {
    const navigate = useNavigate();
    return (
        <GeneratePayroll
            onContinue={() => navigate("/dashboard")}
        />
    );
}

function DashboardWrapper() {
    const navigate = useNavigate();
    return (
        <Dashboard
            onLeaveRequest={() => navigate("/leave")}
            onLogout={() => {
                alert("Đăng xuất thành công!");
                localStorage.removeItem("username");
                navigate("/");
            }}
        />
    );
}

function LeaveWrapper() {
    const navigate = useNavigate();
    return <LeaveRequest onBack={() => navigate("/dashboard")} />;
}



export default App;

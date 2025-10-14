import React from "react";
import { BrowserRouter as Router, Routes, Route, useNavigate, Navigate } from "react-router-dom";

import Header from "./components/Header";
import Calendar from "./components/Calendar";
import ButtonDemo from "./components/ButtonDemo";
import Dashboard from "./pages/Dashboard";
import ViewAdjustmentRequests from "./components/ViewAdjustmentRequests";
import CustomButton from "./components/CustomButton";

import Login from "./pages/Login";
import EmployeeKpiPage from "./pages/EmployeeKpiPage";
import LeaveRequest from "./pages/LeaveRequest";
import GeneratePayroll from "./pages/GeneratePayroll";

import "./App.css";

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
                    path="/attendance"
                    element={
                        <ProtectedRoute>
                            <Calendar />
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

                <Route
                    path="/self-kpi"
                    element={
                        <ProtectedRoute>
                            <EmployeeKpiPage />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/head-kpi"
                    element={
                        <ProtectedRoute>
                            <EmployeeKpiPage />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/manager-kpi"
                    element={
                        <ProtectedRoute>
                            <EmployeeKpiPage />
                        </ProtectedRoute>
                    }
                />
            </Routes>
        </Router>
    );
}

// ---- Route wrappers ----

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
    return <GeneratePayroll onContinue={() => navigate("/dashboard")} />;
}

export default App;

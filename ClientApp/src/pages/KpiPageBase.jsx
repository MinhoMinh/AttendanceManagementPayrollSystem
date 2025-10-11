import React, { useState, useEffect } from "react";
import KpiComponentTable from "../components/KpiComponentTable";
import MonthYearSelector from "../components/MonthYearSelector";

export default function KpiPageBase({ role }) {
    const [selectedEmployee, setSelectedEmployee] = useState(null);
    const [selectedPeriod, setSelectedPeriod] = useState({ month: 10, year: 2025 });
    const [components, setComponents] = useState([]);
    const [mode, setMode] = useState("view");

    useEffect(() => {
        if (selectedEmployee) {
            fetchKpi(selectedEmployee.empId, selectedPeriod.month, selectedPeriod.year);
        }
    }, [selectedEmployee, selectedPeriod]);

    const fetchKpi = async (empId, month, year) => {
        // Fetch from API like `/api/kpi?empId=...&month=...&year=...`
        // Simulate:
        const fake = [
            { kpiComponentId: 1, name: "Sales", description: "Sales performance", targetValue: 100, achievedValue: 80, weight: 50, selfScore: 8, assignedScore: 7 },
            { kpiComponentId: 2, name: "Attendance", description: "On-time presence", targetValue: 30, achievedValue: 29, weight: 50, selfScore: 9, assignedScore: 8 }
        ];
        setComponents(fake);
    };

    const handleSave = (updated) => {
        console.log("Saving KPI for", selectedEmployee, updated);
    };

    // Determine mode by role
    let effectiveMode = "view";
    if (role === "employee") effectiveMode = "self";
    if (role === "dephead") effectiveMode = "view";
    if (role === "manager") effectiveMode = "assign";

    return (
        <div style={{ padding: "20px" }}>
            <h2>KPI Management ({role})</h2>

            <MonthYearSelector
                value={selectedPeriod}
                onChange={(p) => setSelectedPeriod(p)}
            />

            {(role === "dephead" || role === "manager") && (
                <EmployeeSelector
                    role={role}
                    onSelect={setSelectedEmployee}
                />
            )}

            {selectedEmployee && (
                <KpiComponentTable
                    components={components}
                    mode={effectiveMode}
                    onChange={(id, f, v) =>
                        setComponents((prev) =>
                            prev.map((c) =>
                                c.kpiComponentId === id ? { ...c, [f]: v } : c
                            )
                        )
                    }
                    onSave={handleSave}
                />
            )}
        </div>
    );
}

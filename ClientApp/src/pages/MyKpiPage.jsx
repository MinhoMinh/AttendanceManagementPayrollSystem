import React, { useEffect, useState } from "react";
import MonthYearSelector from "./MonthYearSelector";
import EmployeeKpiView from "./EmployeeKpiView";

export default function MyKpiPage() {
    const [employeeKpi, setEmployeeKpi] = useState(null);
    const [month, setMonth] = useState(new Date().getMonth() + 1);
    const [year, setYear] = useState(new Date().getFullYear());
    const [viewMode, setViewMode] = useState("view"); // "view" | "self-score"
    const empId = 1; // replace with logged-in employee ID or context

    useEffect(() => {
        const fetchKpi = async () => {
            try {
                const response = await fetch(
                    `/api/EmployeeKpi/${empId}?month=${month}&year=${year}`
                );

                if (!response.ok) {
                    // 404 or other errors
                    setEmployeeKpi(null);
                    return;
                }

                const data = await response.json();
                setEmployeeKpi(data);
            } catch (error) {
                console.error("Failed to fetch KPI:", error);
            }
        };

        fetchKpi();
    }, [empId, month, year]);

    // Allow self-score mode only 3 days after the month
    const today = new Date();
    const currentMonth = today.getMonth() + 1;
    const currentYear = today.getFullYear();
    const canSelfScore =
        year === currentYear && month === currentMonth && today.getDate() >= 3;

    const toggleMode = () => {
        if (viewMode === "view" && !canSelfScore) {
            alert("Self-scoring is only available starting 3 days after the month begins.");
            return;
        }
        setViewMode((prev) => (prev === "view" ? "self-score" : "view"));
    };

    return (
        <div className="p-6">
            <h1 className="text-xl font-bold mb-4">My KPI</h1>

            <MonthYearSelector
                selectedMonth={month}
                selectedYear={year}
                onChange={(m, y) => {
                    setMonth(m);
                    setYear(y);
                }}
            />

            {!employeeKpi ? (
                <p className="text-gray-500 mt-4">No KPI data available for this period.</p>
            ) : (
                <>
                    <div className="flex justify-between items-center mb-4">
                        <button
                            className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                            onClick={toggleMode}
                        >
                            {viewMode === "view" ? "Switch to Self-Score" : "View Only"}
                        </button>
                    </div>

                    <EmployeeKpiView
                        employeeKpi={employeeKpi}
                        mode={viewMode}
                        onToggleMode={toggleMode}
                    />
                </>
            )}
        </div>
    );
}

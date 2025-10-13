import React, { useEffect, useState } from "react";
import KpiPageBase from "./KpiPageBase";

export default function EmployeeKpiPage() {
    const [kpi, setKpi] = useState(null);
    const [loading, setLoading] = useState(false);
    const [month, setMonth] = useState(new Date().getMonth() + 1);
    const [year, setYear] = useState(new Date().getFullYear());

    // Get employee info from localStorage
    const emp = JSON.parse(localStorage.getItem("employee"));
    const empId = emp?.empId;

    useEffect(() => {
        if (!empId) return;
        fetchKpi(empId, month, year);
    }, [empId, month, year]);

    async function fetchKpi(empId, month, year) {
        setLoading(true);
        try {
            const res = await fetch(`http://localhost:5038/api/kpi/self?empId=${empId}&month=${month}&year=${year}`);
            if (!res.ok) {
                const text = await res.text(); // read server response
                throw new Error(`HTTP ${res.status}: ${text}`);
            }
            const data = await res.json();
            setKpi(data);
        } catch (err) {
            console.error(err);
            setKpi(null);
        } finally {
            setLoading(false);
        }
    }

    function handlePeriodChange(newMonth, newYear) {
        setMonth(newMonth);
        setYear(newYear);
    }

    async function handleSave(updatedComponents) {
        if (!kpi || !empId) return;

        const payload = {
            periodMonth: kpi.periodMonth,
            periodYear: kpi.periodYear,
            components: updatedComponents
        };
        console.log("Payload:", JSON.stringify(payload, null, 2)); // log neatly formatted JSON

        try {
            const res = await fetch(`http://localhost:5038/api/kpi/self/${empId}/score`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const text = await res.text();
                throw new Error(`HTTP ${res.status}: ${text}`);
            }

            const msg = await res.json();
            console.log("Saved successfully:", msg);

            // Refresh state to reflect updated data
            setKpi(prev => ({
                ...prev,
                components: updatedComponents
            }));
        } catch (err) {
            console.error("Error saving self score:", err);
            alert("Lỗi khi lưu KPI: " + err.message);
        }
    }


    if (loading) return <p>Đang tải KPI...</p>;

    return (
        <KpiPageBase
            kpiDto={kpi}
            onPeriodChange={handlePeriodChange}
            onSave={handleSave}
        />
    );
}

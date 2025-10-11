import React, { useEffect, useState } from "react";
import MonthYearSelector from "../components/MonthYearSelector";
import KpiComponentTable from "../components/KpiComponentTable";

export default function EmployeeKPI({ empId }) {
    const [month, setMonth] = useState(new Date().getMonth() + 1);
    const [year, setYear] = useState(new Date().getFullYear());
    const [kpiData, setKpiData] = useState(null);
    const [mode, setMode] = useState("self"); // "view" | "self"
    const [debugOverride, setDebugOverride] = useState(false);
    const [loading, setLoading] = useState(false);

    const fetchKpi = async () => {
        setLoading(true);
        try {
            const res = await fetch(`http://localhost:5038/api/kpi/${empId}?month=${month}&year=${year}`);
            if (!res.ok) throw new Error("Failed to load KPI");
            const data = await res.json();
            setKpiData(data);
            determineMode(data);
        } catch (err) {
            console.error(err);
            alert("Error loading KPI");
        } finally {
            setLoading(false);
        }
    };

    const determineMode = (data) => {
        if (!data || debugOverride) {
            setMode(debugOverride ? "self" : "view");
            return;
        }

        // 🔹 Support both PascalCase (Kpi) and camelCase (kpi)
        const kpi = data.kpi || data.Kpi;
        if (!kpi) {
            setMode("view");
            return;
        }

        const now = new Date();
        const viewedDate = new Date(kpi.periodYear || kpi.PeriodYear, (kpi.periodMonth || kpi.PeriodMonth) - 1, 1);
        const diffDays = Math.abs((now - viewedDate) / (1000 * 60 * 60 * 24));

        // within 3 days from KPI month → selfscore
        if (diffDays <= 3) setMode("selfscore");
        else setMode("view");
    };

    //const handleSave = async () => {
    //    if (!kpiData) return;

    //    const kpi = kpiData.kpi || kpiData.Kpi;
    //    if (!kpi) return;

    //    const components = kpi.components?.$values || kpi.Components?.$values || kpi.components || kpi.Components || [];

    //    try {
    //        const res = await fetch(`http://localhost:5038/api/kpi/${kpi.kpiId || kpi.KpiId}/selfscore`, {
    //            method: "PUT",
    //            headers: { "Content-Type": "application/json" },
    //            body: JSON.stringify(components),
    //        });
    //        if (!res.ok) throw new Error("Failed to save");
    //        alert("Saved successfully!");
    //    } catch (err) {
    //        console.error(err);
    //        alert("Error saving self score.");
    //    }
    //};

    const handleSave = async () => {
        if (!kpiData) return;

        try {
            const res = await fetch(`http://localhost:5038/api/kpi/${empId}/save?phase=self`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(kpiData),
            });

            if (!res.ok) throw new Error(`Failed to save: ${res.statusText}`);

            const saved = await res.json();
            setKpiData(saved);
            alert("✅ Self-score saved successfully!");
        } catch (err) {
            console.error(err);
            alert("❌ Error saving self score.");
        }
    };

    const handleChange = (compId, field, value) => {
        setKpiData((prev) => {
            if (!prev) return prev;

            const updated = { ...prev };
            const kpi = updated.kpi || updated.Kpi;
            const components = kpi.components?.$values || kpi.Components?.$values || [];

            const comp = components.find(
                (c) =>
                    c.kpiComponentId === compId ||
                    c.KpiComponentId === compId
            );

            if (comp) {
                const key = Object.keys(comp).find(
                    (k) => k.toLowerCase() === field.toLowerCase()
                );
                comp[key] = value;
            }

            return { ...updated };
        });
    };

    const kpi = kpiData?.kpi || kpiData?.Kpi;
    const components = kpi?.components?.$values || kpi?.Components?.$values || [];

    return (
        <div className="p-4 space-y-4">
            <h2 className="text-xl font-bold">Employee KPI</h2>

            {/* Controls */}
            <div className="flex justify-between items-center">
                <MonthYearSelector
                    month={month}
                    year={year}
                    onChange={(m, y) => {
                        setMonth(m);
                        setYear(y);
                    }}
                />

                <div className="flex items-center gap-3">
                    <button
                        onClick={fetchKpi}
                        className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
                    >
                        {loading ? "Loading..." : "Load KPI"}
                    </button>

                    <label className="flex items-center gap-1 text-sm">
                        <input
                            type="checkbox"
                            checked={debugOverride}
                            onChange={(e) => {
                                setDebugOverride(e.target.checked);
                                determineMode(kpiData);
                            }}
                        />
                        Debug override (force selfscore)
                    </label>
                </div>
            </div>

            {/* KPI Display */}
            {!kpiData ? (
                <p className="italic text-gray-500">No KPI loaded.</p>
            ) : (
                <>
                    <div className="p-2 border rounded shadow bg-gray-50">
                        <p><strong>Employee:</strong> {kpiData.name || kpiData.Name}</p>
                        <p><strong>Month:</strong> {kpi?.periodMonth || kpi?.PeriodMonth}/{kpi?.periodYear || kpi?.PeriodYear}</p>
                        <p><strong>KPI Rate:</strong> {kpi?.kpiRate || kpi?.KpiRate}</p>
                        <p><strong>Mode:</strong> {mode}</p>
                    </div>

                        <KpiComponentTable
                        employee={kpiData}
                        mode={mode}
                        onChange={handleChange}
                        onSave={handleSave}
                    />
                </>
            )}
        </div>
    );
}

import React, { useState } from "react";
import KpiTable from "../components/KpiTable";

export default function MonthlyKpi({ empId }) {
    const role = "Head"; // can also be passed as a prop

    const [month, setMonth] = useState(new Date().getMonth() + 1);
    const [year, setYear] = useState(new Date().getFullYear());
    const [kpiData, setKpiData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    // phase can be "Assign", "SelfScore", "Finalize", "ViewOnly"
    const [phase, setPhase] = useState("SelfScore");

    const fetchKpi = async () => {
        try {
            setLoading(true);
            setError(null);

            const res = await fetch(
                `http://localhost:5038/api/kpi/${empId}?month=${month}&year=${year}`
            );
            if (!res.ok) throw new Error(`Failed to fetch: ${res.statusText}`);
            const data = await res.json();

            const kpisArray = data.kpis?.$values ?? [];
            const kpis = kpisArray.map((k) => ({
                ...k,
                components: k.components?.$values ?? [],
            }));

            setKpiData({ ...data, kpis });
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    const handleKpiChange = (compId, field, value) => {
        setKpiData((prev) => {
            if (!prev) return prev;
            const newComponents = prev.kpis[0].components.map((comp) =>
                comp.kpiComponentId === compId ? { ...comp, [field]: value } : comp
            );
            const newKpis = [...prev.kpis];
            newKpis[0] = { ...newKpis[0], components: newComponents };
            return { ...prev, kpis: newKpis };
        });
    };

    const handleSave = async (updatedKpi) => {
        // --- Validation ---
        const totalWeight = updatedKpi.components.reduce((sum, c) => sum + Number(c.weight), 0);
        if (phase === "Assign" && totalWeight !== 100) {
            alert("Total weight of all components must be 100.");
            return;
        }

        const invalidScore = updatedKpi.components.some(
            (c) => c.selfScore < 0 || c.selfScore > 10 ||
                c.assignedScore < 0 || c.assignedScore > 10 ||
                c.achievedValue < 0 
        );
        if ((phase === "SelfScore" || phase === "Finalize") && invalidScore) {
            alert("Scores must be between 0 and 10.");
            return;
        }

        // --- proceed with save ---
        try {
            setLoading(true);
            setError(null);

            const res = await fetch(
                `http://localhost:5038/api/kpi/${empId}/save?phase=${phase}`,
                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        EmpId: empId,
                        Name: kpiData?.Name ?? "",
                        Kpis: [updatedKpi]
                    })
                }
            );

            if (!res.ok) throw new Error(`Failed to save: ${res.statusText}`);
            const savedData = await res.json();

            const kpisArray = savedData.kpis?.$values ?? [];
            const kpis = kpisArray.map((k) => ({
                ...k,
                components: k.components?.$values ?? [],
            }));
            setKpiData({ ...savedData, kpis });
            alert("KPI saved successfully!");
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };


    const currentKpi = kpiData?.kpis?.[0];

    return (
        <div className="p-4">
            <div className="mb-4 flex gap-2 items-center">
                <label>Month:</label>
                <select value={month} onChange={(e) => setMonth(Number(e.target.value))}>
                    {[...Array(12)].map((_, i) => (
                        <option key={i + 1} value={i + 1}>{i + 1}</option>
                    ))}
                </select>

                <label>Year:</label>
                <select value={year} onChange={(e) => setYear(Number(e.target.value))}>
                    {Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - i).map((y) => (
                        <option key={y} value={y}>{y}</option>
                    ))}
                </select>

                <label>Phase:</label>
                <select value={phase} onChange={(e) => setPhase(e.target.value)}>
                    <option value="Assign">Assign</option>
                    <option value="SelfScore">SelfScore</option>
                    <option value="Finalize">Finalize</option>
                    <option value="ViewOnly">View Only</option>
                </select>

                <button
                    onClick={fetchKpi}
                    className="bg-blue-600 text-white px-3 py-1 rounded hover:bg-blue-700"
                >
                    View KPI
                </button>
            </div>

            {loading && <div>Loading KPI data...</div>}
            {error && <div className="text-red-600">Error: {error}</div>}
            {!loading && !error && (!currentKpi || !currentKpi.components.length) && (
                <div>No KPI data available.</div>
            )}

            {currentKpi && currentKpi.components.length > 0 && (
                <KpiTable
                    kpi={currentKpi}
                    role={role}
                    phase={phase}
                    onChange={handleKpiChange}
                    onSave={handleSave}
                />
            )}
        </div>
    );
}

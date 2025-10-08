import React, { useState } from "react";
import PayrollView from "../components/PayrollView";

function GeneratePayroll() {
    const [month, setMonth] = useState("");
    const [year, setYear] = useState("");
    const [loading, setLoading] = useState(false);
    const [payroll, setPayroll] = useState(null);
    const [error, setError] = useState("");

    const handleGenerate = async () => {
        if (!month || !year) {
            setError("Please enter both month and year.");
            return;
        }

        setError("");
        setLoading(true);
        setPayroll(null);

        try {
            const response = await fetch("http://localhost:5038/api/payroll/generate", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name: `Payroll-${month}-${year}`,
                    periodMonth: parseInt(month),
                    periodYear: parseInt(year),
                    createdBy: 3,
                }),
            });

            if (!response.ok) {
                throw new Error(`Failed: ${response.status}`);
            }

            const data = await response.json();
            setPayroll(data);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ padding: "2rem" }}>
            <h1>Payroll Generation</h1>

            <div style={{ marginBottom: "1rem" }}>
                <label>
                    Month:{" "}
                    <input
                        type="number"
                        value={month}
                        onChange={(e) => setMonth(e.target.value)}
                        placeholder="e.g. 10"
                    />
                </label>
                <br />
                <label>
                    Year:{" "}
                    <input
                        type="number"
                        value={year}
                        onChange={(e) => setYear(e.target.value)}
                        placeholder="e.g. 2025"
                    />
                </label>
                <br />
                <button onClick={handleGenerate} disabled={loading}>
                    {loading ? "Generating..." : "Generate Payroll"}
                </button>
            </div>

            {/* 🔸 Show errors */}
            {error && <p style={{ color: "red" }}>Error: {error}</p>}

            {/* 🔸 Show payroll only if success and not loading */}
            {!loading && !error && payroll && (
                <PayrollView payroll={payroll} />
            )}
        </div>
    );
}

export default GeneratePayroll;
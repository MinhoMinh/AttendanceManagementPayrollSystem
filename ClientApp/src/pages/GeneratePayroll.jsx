import React, { useState } from "react";

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
            const response = await fetch(`http://localhost:5038/api/payroll/generate`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    "name": "payroll",
                    "periodMonth": 10,
                    "periodYear": 2025,
                    "createdBy": 3
                })
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

            {error && <p style={{ color: "red" }}>Error: {error}</p>}

            {payroll && (
                <div>
                    <h2>Payroll Run: {payroll.name || "(unnamed)"}</h2>
                    <p>
                        Period: {payroll.periodMonth}/{payroll.periodYear}
                    </p>
                    <p>Status: {payroll.status}</p>

                    <h3>Employee Salary Details</h3>
                    <table border="1" cellPadding="8">
                        <thead>
                            <tr>
                                <th>Employee ID</th>
                                <th>Base Pay</th>
                                <th>Gross Salary</th>
                                <th>Deductions</th>
                                <th>Net Pay</th>
                            </tr>
                        </thead>
                        <tbody>
                            {payroll.previews?.$values?.map((item) => (
                                <tr key={item.empId}>
                                    <td>{item.empId}</td>
                                    <td>{item.baseSalary}</td>
                                    <td>{item.grossSalary}</td>
                                    <td>{item.totalDeductions}</td>
                                    <td>{item.netSalary}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );

}

export default GeneratePayroll;
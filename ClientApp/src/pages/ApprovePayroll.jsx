import React, { useEffect, useState } from "react";
import PayrollView from "../components/PayrollView";

function ApprovePayroll() {
    const [payrolls, setPayrolls] = useState([]);
    const [selectedPayroll, setSelectedPayroll] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const fetchPayrolls = async () => {
        try {
            setLoading(true);
            const response = await fetch("http://localhost:5038/api/payroll");
            if (!response.ok) throw new Error("Failed to fetch payrolls");
            const data = await response.json();
            setPayrolls(data.$values || data); // handle .NET collection
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPayrolls();
    }, []);

    const handleApprove = async (id, stage) => {
        const userId = 3; // TODO: get from auth context
        const url =
            stage === "first"
                ? `http://localhost:5038/api/payroll/approve/first/${id}`
                : `http://localhost:5038/api/payroll/approve/final/${id}`;

        try {
            console.log("payrollRunId:", id);
            console.log("userId:", userId);

            const res = await fetch(url, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ approvedBy: userId }),
            });
            if (!res.ok) throw new Error("Failed to approve");
            await fetchPayrolls(); // refresh
        } catch (err) {
            alert(err.message);
        }
    };

    const handleReject = async (id) => {
        const userId = 3;
        try {
            const res = await fetch(`http://localhost:5038/api/payroll/reject/${id}`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ approvedBy: userId }),
            });
            if (!res.ok) throw new Error("Failed to reject payroll");
            await fetchPayrolls();
        } catch (err) {
            alert(err.message);
        }
    };

    return (
        <div style={{ padding: "2rem" }}>
            <h1>Approve Payroll</h1>

            {loading && <p>Loading payrolls...</p>}
            {error && <p style={{ color: "red" }}>{error}</p>}

            {!loading && payrolls.length > 0 && (
                <table border="1" cellPadding="8" width="100%">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Period</th>
                            <th>Status</th>
                            <th>Approvals</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {payrolls.map((p) => (
                            <tr key={p.payrollRunId}>
                                <td>{p.payrollRunId}</td>
                                <td>{p.name}</td>
                                <td>
                                    {p.periodMonth}/{p.periodYear}
                                </td>
                                <td>{p.status || p.approvalStatus}</td>
                                <td>
                                    1st: {p.approvedFirstBy || "-"} | Final:{" "}
                                    {p.approvedFinalBy || "-"}
                                </td>
                                <td>
                                    <button onClick={() => setSelectedPayroll(p)}>View</button>{" "}
                                    <button onClick={() => handleApprove(p.payrollRunId, "first")}>
                                        1st Approve
                                    </button>{" "}
                                    <button onClick={() => handleApprove(p.payrollRunId, "final")}>
                                        Final Approve
                                    </button>{" "}
                                    <button onClick={() => handleReject(p.payrollRunId)}>Reject</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}

            {selectedPayroll && (
                <div style={{ marginTop: "2rem", borderTop: "1px solid #ccc", paddingTop: "1rem" }}>
                    <PayrollView payroll={selectedPayroll} />
                    <button onClick={() => setSelectedPayroll(null)}>Close</button>
                </div>
            )}
        </div>
    );
}

export default ApprovePayroll;

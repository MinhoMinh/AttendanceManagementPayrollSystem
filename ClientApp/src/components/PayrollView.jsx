import React from "react";



function PayrollView({ payroll }) {
    if (!payroll) return <p>No payroll data available.</p>;

    return (
        <div style={{ marginTop: "2rem" }}>
            <h2>Payroll Run: {payroll.name || "(Unnamed)"}</h2>
            <p>
                Period: {payroll.periodMonth}/{payroll.periodYear}
            </p>
            <p>Status: {payroll.status || payroll.approvalStatus}</p>
            <p>First Approved By: {payroll.approvedFirstBy ?? "—"}</p>
            <p>Final Approved By: {payroll.approvedFinalBy ?? "—"}</p>


            <h3>Employee Salary Details</h3>
            <table border="1" cellPadding="8" style={{ borderCollapse: "collapse", width: "100%" }}>
                <thead>
                    <tr>
                        <th>Employee ID</th>
                        <th>Base Salary</th>
                        <th>Gross Salary</th>
                        <th>Deductions</th>
                        <th>Net Salary</th>
                    </tr>
                </thead>
                <tbody>
                    {payroll.previews?.$values?.length > 0 ? (
                        payroll.previews.$values.map((item) => (
                            <tr key={item.empId}>
                                <td>{item.empId}</td>
                                <td>{item.baseSalary.toLocaleString()}</td>
                                <td>{item.grossSalary.toLocaleString()}</td>
                                <td>{item.totalDeductions.toLocaleString()}</td>
                                <td>{item.netSalary.toLocaleString()}</td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="5" style={{ textAlign: "center" }}>
                                No salary previews found.
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default PayrollView;
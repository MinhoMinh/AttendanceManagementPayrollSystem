import { useState } from "react";

function AddAllowanceTypeForm() {
    const [formData, setFormData] = useState({
        typeName: "",
        calculationType: "",
        effectiveStartDate: "",
        value: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const newAllowance = {
            typeName: formData.typeName.trim(),
            calculationType: formData.calculationType,
            effectiveStartDate: formData.effectiveStartDate,
            value: parseFloat(formData.value),
            createdAt: new Date().toISOString(), // tùy backend có nhận trường này không
        };

        try {
            const response = await fetch("https://localhost:7184/api/allowancetype", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(newAllowance),
            });

            if (response.ok) {
                alert("Added successfully!");
                setFormData({
                    typeName: "",
                    calculationType: "",
                    effectiveStartDate: "",
                    value: "",
                });
            } else {
                const err = await response.text();
                alert("Failed to add. " + err);
            }
        } catch (error) {
            alert("Error: " + error.message);
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            style={{
                display: "flex",
                flexDirection: "column",
                gap: "10px",
                width: "320px",
                margin: "20px auto",
                padding: "20px",
                border: "1px solid #ccc",
                borderRadius: "8px",
                background: "#f9f9f9",
            }}
        >
            <h3 style={{ textAlign: "center" }}>Add Allowance Type</h3>

            <input
                type="text"
                name="typeName"
                value={formData.typeName}
                onChange={handleChange}
                placeholder="Allowance type name"
                required
            />

            <select
                name="calculationType"
                value={formData.calculationType}
                onChange={handleChange}
                required
            >
                <option value="">-- Select Calculation Type --</option>
                <option value="Fixed">Fixed</option>
                <option value="Percentage">Percentage</option>
            </select>

            <input
                type="date"
                name="effectiveStartDate"
                value={formData.effectiveStartDate}
                onChange={handleChange}
                required
            />

            <input
                type="number"
                name="value"
                value={formData.value}
                onChange={handleChange}
                placeholder="Value"
                step="0.01"
                required
            />

            <button
                type="submit"
                style={{
                    padding: "8px 12px",
                    border: "none",
                    background: "#007bff",
                    color: "white",
                    borderRadius: "4px",
                    cursor: "pointer",
                }}
            >
                Add Allowance
            </button>
        </form>
    );
}

export default AddAllowanceTypeForm;

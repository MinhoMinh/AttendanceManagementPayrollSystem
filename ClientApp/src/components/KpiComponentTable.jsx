import { useState, useEffect } from "react";

export default function KpiComponentTable({ components, mode = "view", onSave }) {
    const editable = mode !== "view";
    const [localComponents, setLocalComponents] = useState(components || []);
    const [error, setError] = useState("");

    useEffect(() => {
        setLocalComponents(components || []);
    }, [components]);

    const handleChange = (id, field, value) => {
        let val = value;
        if (["selfScore", "assignedScore"].includes(field)) {
            val = parseFloat(value);
            if (val < 0) val = 0;
            if (val > 10) val = 10;
        }

        const updated = localComponents.map((c) =>
            c.kpiComponentId === id ? { ...c, [field]: val } : c
        );
        setLocalComponents(updated);
    };

    const handleAdd = () => {
        const newItem = {
            kpiComponentId: Date.now(),
            name: "",
            description: "",
            targetValue: 0,
            achievedValue: null,
            weight: 0,
            selfScore: null,
            assignedScore: null,
        };
        setLocalComponents([...localComponents, newItem]);
    };

    const handleRemove = (id) => {
        const updated = localComponents.filter((c) => c.kpiComponentId !== id);
        setLocalComponents(updated);
    };

    const isEditable = (field) => {
        switch (mode) {
            case "self":
                return ["selfScore", "achievedValue"].includes(field);
            case "edit":
                return !["selfScore", "assignedScore", "achievedValue"].includes(field);
            case "assign":
                return field === "assignedScore";
            default:
                return false;
        }
    };

    const handleSave = () => {
        if (mode === "edit") {
            const totalWeight = localComponents.reduce(
                (sum, c) => sum + Number(c.weight || 0),
                0
            );
            if (totalWeight !== 100) {
                setError("Total weight of all components must equal 100.");
                return;
            }
        }
        setError("");
        onSave?.(localComponents);
    };

    return (
        <div>
            <table
                border="1"
                cellPadding="5"
                style={{ borderCollapse: "collapse", width: "100%" }}
            >
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Target</th>
                        <th>Achieved</th>
                        <th>Weight</th>
                        <th>Self Score</th>
                        <th>Assigned Score</th>
                        {mode === "edit" && <th>Action</th>}
                    </tr>
                </thead>
                <tbody>
                    {localComponents.map((item) => (
                        <tr key={item.kpiComponentId}>
                            {[
                                "name",
                                "description",
                                "targetValue",
                                "achievedValue",
                                "weight",
                                "selfScore",
                                "assignedScore",
                            ].map((field) => (
                                <td key={field}>
                                    {isEditable(field) ? (
                                        <input
                                            type={
                                                ["targetValue", "achievedValue", "weight", "selfScore", "assignedScore"].includes(field)
                                                    ? "number"
                                                    : "text"
                                            }
                                            value={item[field] ?? ""}
                                            onChange={(e) =>
                                                handleChange(item.kpiComponentId, field, e.target.value)
                                            }
                                            min={["selfScore", "assignedScore"].includes(field) ? "0" : undefined}
                                            max={["selfScore", "assignedScore"].includes(field) ? "10" : undefined}
                                        />
                                    ) : (
                                        item[field] ?? "-"
                                    )}
                                </td>
                            ))}
                            {mode === "edit" && (
                                <td>
                                    <button onClick={() => handleRemove(item.kpiComponentId)}>
                                        Remove
                                    </button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>

            {error && <div style={{ color: "red", marginTop: "10px" }}>{error}</div>}

            {mode === "edit" && (
                <div style={{ marginTop: "10px" }}>
                    <button onClick={handleAdd}>Add Component</button>
                </div>
            )}

            {editable && (
                <button
                    onClick={() => onSave(localComponents)}
                    style={{ marginTop: "10px", padding: "8px 14px" }}
                >
                    💾 Lưu thay đổi
                </button>
            )}
        </div>
    );
}

import { useState } from "react";

export default function KpiComponentTable({ components, mode = "view", onChange, onSave }) {
    const [localComponents, setLocalComponents] = useState(components || []);
    const [error, setError] = useState("");

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
        if (onChange) onChange(id, field, val);
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
        if (onSave) onSave(localComponents);
    };

    return (
        <div>
            <table border="1" cellPadding="5" style={{ borderCollapse: "collapse", width: "100%" }}>
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
                            <td>
                                {isEditable("name") ? (
                                    <input
                                        value={item.name}
                                        onChange={(e) => handleChange(item.kpiComponentId, "name", e.target.value)}
                                    />
                                ) : (
                                    item.name
                                )}
                            </td>
                            <td>
                                {isEditable("description") ? (
                                    <input
                                        value={item.description}
                                        onChange={(e) => handleChange(item.kpiComponentId, "description", e.target.value)}
                                    />
                                ) : (
                                    item.description
                                )}
                            </td>
                            <td>
                                {isEditable("targetValue") ? (
                                    <input
                                        type="number"
                                        value={item.targetValue}
                                        onChange={(e) => handleChange(item.kpiComponentId, "targetValue", e.target.value)}
                                    />
                                ) : (
                                    item.targetValue
                                )}
                            </td>
                            <td>
                                {isEditable("achievedValue") ? (
                                    <input
                                        type="number"
                                        value={item.achievedValue ?? ""}
                                        onChange={(e) => handleChange(item.kpiComponentId, "achievedValue", e.target.value)}
                                    />
                                ) : (
                                    item.achievedValue ?? "-"
                                )}
                            </td>
                            <td>
                                {isEditable("weight") ? (
                                    <input
                                        type="number"
                                        value={item.weight}
                                        onChange={(e) => handleChange(item.kpiComponentId, "weight", e.target.value)}
                                    />
                                ) : (
                                    item.weight
                                )}
                            </td>
                            <td>
                                {isEditable("selfScore") ? (
                                    <input
                                        type="number"
                                        min="0"
                                        max="10"
                                        value={item.selfScore ?? ""}
                                        onChange={(e) => handleChange(item.kpiComponentId, "selfScore", e.target.value)}
                                    />
                                ) : (
                                    item.selfScore ?? "-"
                                )}
                            </td>
                            <td>
                                {isEditable("assignedScore") ? (
                                    <input
                                        type="number"
                                        min="0"
                                        max="10"
                                        value={item.assignedScore ?? ""}
                                        onChange={(e) => handleChange(item.kpiComponentId, "assignedScore", e.target.value)}
                                    />
                                ) : (
                                    item.assignedScore ?? "-"
                                )}
                            </td>
                            {mode === "edit" && (
                                <td>
                                    <button onClick={() => handleRemove(item.kpiComponentId)}>Remove</button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>

            {error && (
                <div style={{ color: "red", marginTop: "10px" }}>{error}</div>
            )}

            {mode === "edit" && (
                <div style={{ marginTop: "10px" }}>
                    <button onClick={handleAdd}>Add Component</button>
                </div>
            )}

            {mode !== "view" && onSave && (
                <div style={{ marginTop: "10px" }}>
                    <button onClick={handleSave}>Save</button>
                </div>
            )}
        </div>
    );
}

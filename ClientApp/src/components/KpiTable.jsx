import React, { useState } from "react";

export default function KpiTable({ kpi, role, phase, onChange, onSave }) {
    const [localKpi, setLocalKpi] = useState(kpi);

    // Update local state when kpi prop changes
    React.useEffect(() => {
        setLocalKpi(kpi);
    }, [kpi]);

    const handleChange = (compId, field, value) => {
        const updatedComponents = localKpi.components.map((comp) =>
            comp.kpiComponentId === compId ? { ...comp, [field]: value } : comp
        );
        setLocalKpi({ ...localKpi, components: updatedComponents });
        onChange?.(compId, field, value);
    };

    const handleAddComponent = () => {
        const newComponent = {
            kpiComponentId: Date.now(), // temporary ID, replace with server ID after save
            name: "",
            description: "",
            targetValue: 0,
            achievedValue: 0,
            weight: 0,
            selfScore: 0,
            assignedScore: 0,
        };
        setLocalKpi({
            ...localKpi,
            components: [...localKpi.components, newComponent],
        });
    };

    const handleRemoveComponent = (compId) => {
        setLocalKpi({
            ...localKpi,
            components: localKpi.components.filter((c) => c.kpiComponentId !== compId),
        });
    };

    if (!localKpi || !localKpi.components?.length) {
        return <div>No KPI components available.</div>;
    }

    const canEditComponentFields = role === "Head" && phase === "Assign";
    const canSelfScore = role === "Employee" && phase === "SelfScore";
    const canAssignScore = role === "Head" && phase === "Finalize";
    const isViewOnly = phase === "ViewOnly";

    return (
        <div className="border-l-4 border-blue-400 pl-4 mb-4">
            <h3 className="text-blue-600 font-medium">
                KPI #{localKpi.kpiId} — {localKpi.periodMonth}/{localKpi.periodYear}
            </h3>
            <p className="text-sm mb-2">
                Rate: <strong>{localKpi.kpiRate}</strong>
            </p>

            {(canEditComponentFields || canSelfScore || canAssignScore) && (
                <div className="mb-2 flex gap-2">
                    {canEditComponentFields && (
                        <button
                            className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
                            onClick={handleAddComponent}
                        >
                            Add Component
                        </button>
                    )}
                    <button
                        className="bg-blue-600 text-white px-3 py-1 rounded hover:bg-blue-700"
                        onClick={() => onSave?.(localKpi)}
                    >
                        Save
                    </button>
                </div>
            )}

            <table className="w-full text-sm border border-gray-300 rounded">
                <thead className="bg-gray-100">
                    <tr>
                        <th className="border px-2 py-1 text-left">Name</th>
                        <th className="border px-2 py-1 text-left">Description</th>
                        <th className="border px-2 py-1">Target</th>
                        <th className="border px-2 py-1">Achieved</th>
                        <th className="border px-2 py-1">Weight</th>
                        <th className="border px-2 py-1">Self</th>
                        <th className="border px-2 py-1">Assigned</th>
                        {canEditComponentFields && <th className="border px-2 py-1">Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {localKpi.components.map((comp) => (
                        <tr key={comp.kpiComponentId}>
                            <td className="border px-2 py-1">
                                {(canEditComponentFields && !isViewOnly) ? (
                                    <input
                                        className="border p-1 w-full"
                                        value={comp.name}
                                        onChange={(e) =>
                                            handleChange(comp.kpiComponentId, "name", e.target.value)
                                        }
                                    />
                                ) : (
                                    comp.name
                                )}
                            </td>
                            <td className="border px-2 py-1">
                                {(canEditComponentFields && !isViewOnly) ? (
                                    <textarea
                                        className="border p-1 w-full"
                                        value={comp.description}
                                        onChange={(e) =>
                                            handleChange(comp.kpiComponentId, "description", e.target.value)
                                        }
                                    />
                                ) : (
                                    comp.description
                                )}
                            </td>
                            <td className="border px-2 py-1 text-center">
                                {(canEditComponentFields && !isViewOnly) ? (
                                    <input
                                        type="number"
                                        className="border p-1 w-20 text-center"
                                        value={comp.targetValue}
                                        onChange={(e) =>
                                            handleChange(comp.kpiComponentId, "targetValue", e.target.value)
                                        }
                                    />
                                ) : (
                                    comp.targetValue
                                )}
                            </td>
                            <td className="border px-2 py-1 text-center">
                                <input
                                    type="number"
                                    className="border p-1 w-20 text-center"
                                    value={comp.achievedValue}
                                    onChange={(e) =>
                                        handleChange(comp.kpiComponentId, "achievedValue", e.target.value)
                                    }
                                    disabled={!canSelfScore && !canAssignScore || isViewOnly}
                                />
                            </td>
                            <td className="border px-2 py-1 text-center">
                                {(canEditComponentFields && !isViewOnly) ? (
                                    <input
                                        type="number"
                                        className="border p-1 w-16 text-center"
                                        value={comp.weight}
                                        onChange={(e) =>
                                            handleChange(comp.kpiComponentId, "weight", e.target.value)
                                        }
                                    />
                                ) : (
                                    comp.weight
                                )}
                            </td>
                            <td className="border px-2 py-1 text-center">
                                <input
                                    type="number"
                                    step="0.1"
                                    className="border p-1 w-16 text-center"
                                    value={comp.selfScore}
                                    onChange={(e) =>
                                        handleChange(comp.kpiComponentId, "selfScore", e.target.value)
                                    }
                                    disabled={!canSelfScore || isViewOnly}
                                />
                            </td>
                            <td className="border px-2 py-1 text-center">
                                <input
                                    type="number"
                                    step="0.1"
                                    className="border p-1 w-16 text-center"
                                    value={comp.assignedScore}
                                    onChange={(e) =>
                                        handleChange(comp.kpiComponentId, "assignedScore", e.target.value)
                                    }
                                    disabled={!canAssignScore || isViewOnly}
                                />
                            </td>
                            {canEditComponentFields && !isViewOnly && (
                                <td className="border px-2 py-1 text-center">
                                    <button
                                        className="bg-red-600 text-white px-2 py-0.5 rounded hover:bg-red-700"
                                        onClick={() => handleRemoveComponent(comp.kpiComponentId)}
                                    >
                                        Remove
                                    </button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

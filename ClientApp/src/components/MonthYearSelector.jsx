import React from "react";

export default function MonthYearSelector({ value, onSearch }) {
    const months = Array.from({ length: 12 }, (_, i) => i + 1);
    const years = [2024, 2025, 2026];

    const [month, setMonth] = React.useState(value?.month || new Date().getMonth() + 1);
    const [year, setYear] = React.useState(value?.year || new Date().getFullYear());

    React.useEffect(() => {
        if (value) {
            setMonth(value.month);
            setYear(value.year);
        }
    }, [value]);

    return (
        <div style={{ marginBottom: "10px" }}>
            <select value={month} onChange={(e) => setMonth(parseInt(e.target.value))}>
                {months.map((m) => (
                    <option key={m} value={m}>{m}</option>
                ))}
            </select>

            <select value={year} onChange={(e) => setYear(parseInt(e.target.value))}>
                {years.map((y) => (
                    <option key={y} value={y}>{y}</option>
                ))}
            </select>

            <button
                type="button"
                onClick={() => onSearch?.(month, year)}
                style={{ marginLeft: "8px", padding: "6px 12px" }}
            >
                🔍 Tìm kiếm
            </button>
        </div>
    );
}

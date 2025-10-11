export default function MonthYearSelector({ value, onChange }) {
    const months = Array.from({ length: 12 }, (_, i) => i + 1);
    const years = [2024, 2025, 2026];

    return (
        <div style={{ marginBottom: "10px" }}>
            <select
                value={value.month}
                onChange={(e) => onChange({ ...value, month: parseInt(e.target.value) })}
            >
                {months.map((m) => (
                    <option key={m} value={m}>{m}</option>
                ))}
            </select>

            <select
                value={value.year}
                onChange={(e) => onChange({ ...value, year: parseInt(e.target.value) })}
            >
                {years.map((y) => (
                    <option key={y} value={y}>{y}</option>
                ))}
            </select>
        </div>
    );
}

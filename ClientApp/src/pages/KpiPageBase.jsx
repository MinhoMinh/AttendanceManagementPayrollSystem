import React from "react";
import KpiComponentTable from "../components/KpiComponentTable";
import MonthYearSelector from "../components/MonthYearSelector";

export default function KpiPageBase({
  kpiDto,
  onPeriodChange,  // (month, year) => void
  onSave,           // (updatedComponents) => void
  onAddComponent    // optional: () => void
}) {
  // If nothing yet but mode allows creation
  if (!kpiDto) {
    return (
      <div style={{ padding: "20px" }}>
        <h2>KPI</h2>
            <MonthYearSelector
                value={{
                    month: kpiDto?.periodMonth ?? new Date().getMonth(),
                    year: kpiDto?.periodYear ?? new Date().getFullYear(),
                }}
                onSearch={onPeriodChange}
            />


        <p>Không có dữ liệu KPI cho giai đoạn này.</p>

      </div>
    );
  }

  const { periodMonth, periodYear, components, kpiMode } = kpiDto;

  return (
    <div style={{ padding: "20px" }}>
      <h2>KPI {periodMonth}/{periodYear} — Mode: {kpiMode}</h2>

      <MonthYearSelector
        value={{ month: periodMonth, year: periodYear }}
              onSearch={onPeriodChange}
      />


        <KpiComponentTable
            components={components}
            mode={kpiMode}
            onChange={(id, field, value) => {
                const updated = components.map(c =>
                    c.kpiComponentId === id ? { ...c, [field]: value } : c
                );
                onSave(updated);
            }}
            onSave={() => onSave(components)}
        />

      {kpiMode === "assign" && (
        <button
          onClick={onAddComponent}
          style={{ marginTop: "15px", padding: "8px 14px" }}
        >
          Thêm thành phần KPI
        </button>
      )}
    </div>
  );
}

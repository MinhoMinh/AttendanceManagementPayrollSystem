import React, { useState } from "react";
import './Calendar.css';

const mockData = {
  "2025-09": {
    "2025-09-22": { score: 1.0, times: ["07:58", "12:01", "13:00", "17:01"] },
    "2025-09-23": { score: 0.98, times: ["08:10", "12:05", "13:05", "17:02"] },
    "2025-09-24": { score: 0.95, times: ["08:15", "12:10", "13:10", "17:05"] },
    "2025-09-25": { score: 1.0, times: ["07:55", "12:00", "13:00", "17:00"] },
    "2025-09-26": { score: 0.92, times: ["08:20", "12:15", "13:15", "17:10"] },
  },
};

const now = new Date();
const defaultMonth = `${now.getFullYear()}-${String(now.getMonth()+1).padStart(2,'0')}`;

export default function CheckAttendance() {
  const [month, setMonth] = useState(defaultMonth);
  const [selectedDay, setSelectedDay] = useState(null);

  const [yearStr, monthStr] = month.split("-");
  const year = parseInt(yearStr, 10);
  const monthIndex = parseInt(monthStr, 10) - 1;

  const daysInMonth = new Date(year, monthIndex + 1, 0).getDate();
  const firstDay = new Date(year, monthIndex, 1).getDay();
  const startOffset = firstDay === 0 ? 6 : firstDay - 1;

  const weeks = [];
  let currentDay = 1 - startOffset;
  while (currentDay <= daysInMonth) {
    const week = [];
    for (let i = 0; i < 7; i++) {
      if (currentDay > 0 && currentDay <= daysInMonth) week.push(currentDay);
      else week.push(null);
      currentDay++;
    }
    weeks.push(week);
  }

  const onMonthChange = (e) => {
    setMonth(e.target.value);
    setSelectedDay(null);
  };

  return (
    <div className="amps-wrapper">
      <h2 className="amps-title">📅 Check Attendance</h2>

      <div className="amps-filter">
        <label>Chọn tháng:</label>
        <input
          className="amps-input-month"
          type="month"
          value={month}
          onChange={onMonthChange}
        />
      </div>

      <div className="amps-calendar-container">
        <div className="amps-weekdays">
          <div>T2</div>
          <div>T3</div>
          <div>T4</div>
          <div>T5</div>
          <div>T6</div>
          <div>T7</div>
          <div>CN</div>
        </div>

        <div className="amps-calendar-grid">
          {weeks.map((week, wi) =>
            week.map((day, di) => {
              if (!day) return <div key={`${wi}-${di}`} className="day-empty" />;
              const dateKey = `${month}-${String(day).padStart(2, "0")}`;
              const data = mockData[month]?.[dateKey];
              const isSelected = selectedDay?.dateKey === dateKey;

              return (
                <div
                  key={`${wi}-${di}`}
                  className={`day-cell ${isSelected ? "selected" : ""}`}
                  onClick={() => setSelectedDay({ dateKey, data })}
                >
                  <div className="day-number">{day}</div>
                  <div className="day-score">{data?.score ?? ""}</div>
                </div>
              );
            })
          )}
        </div>
      </div>

      {selectedDay && (
        <div className="amps-times-card">
          <div className="times-header">
            <strong>Ngày {selectedDay.dateKey}</strong>
            <span className="score-pill">
              Điểm: {selectedDay.data?.score ?? "—"}
            </span>
          </div>

          <ul className="times-list">
            {selectedDay.data?.times?.length ? (
              selectedDay.data.times.map((t, i) => (
                <li key={i} className="time-item">
                  ⏰ {t}
                </li>
              ))
            ) : (
              <li className="time-empty">Không có dữ liệu</li>
            )}
          </ul>
        </div>
      )}
    </div>
  );
}

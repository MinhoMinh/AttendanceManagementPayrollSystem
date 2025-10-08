import React, { useState, useEffect } from "react";
import './Calendar.css';

export default function CheckAttendance() {
    const now = new Date();
    const defaultMonth = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`;
    const [month, setMonth] = useState(defaultMonth);
    const [selectedDay, setSelectedDay] = useState(null);
    const [attendanceData, setAttendanceData] = useState({}); // <-- dữ liệu thật
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    useEffect(() => {
        console.log("attendanceData updated:", attendanceData);
        console.log("log", attendanceData["2025-10-1"])
    }, [attendanceData]);

    // tách năm/tháng
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

    // 🔹 Gọi API khi month thay đổi
    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                setError("");

                const [y, m] = month.split("-");
                const url = `https://localhost:7184/api/clockin/employee/4?month=${parseInt(m)}&year=${y}`;
                const res = await fetch(url);

                if (!res.ok) throw new Error(`Lỗi HTTP ${res.status}`);
                const json = await res.json();

                // 🔹 Chuyển dữ liệu JSON thành dạng { 'yyyy-mm-dd': { score, times } }
                const result = {};
                if (json?.$values?.length) {
                    json.$values.forEach((record) => {
                        const dateStr = record.date.split("T")[0];
                        if (record.dailyRecords?.$values) {
                            record.dailyRecords.$values.forEach((dayRecord, index) => {
                                const [year, month] = record.date.split("T")[0].split("-");
                                const day = String(dayRecord.day).padStart(2, '0');
                                console.log(typeof day)
                                const key = `${year}-${month}-${day}`;
                                result[key] = {
                                    score: dayRecord.actual,
                                    times: dayRecord.logs?.$values || [],
                                };
                            });
                        }
                    });
                }
                //console.table(Object.entries(result).map(([date, v]) => ({
                //    date,
                //    score: v.score,
                //    logs: v.times.join(", "),
                //})));
                console.log("aaa")
                setAttendanceData(result);
            } catch (err) {
                console.error(err);
                setError("Không thể tải dữ liệu chấm công.");
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [month]);

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

            {loading && <p>Đang tải dữ liệu...</p>}
            {error && <p className="error-text">{error}</p>}

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
                            if (!day)
                                return <div key={`${wi}-${di}`} className="day-empty" />;

                            const dateKey = `${month}-${String(day).padStart(2, "0")}`;
                            //console.log(dateKey)
                            const data = attendanceData[dateKey];
                            //console.log(`${attendanceData.length}`)
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
                                <li key={i} className="time-item">⏰ {t}</li>
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

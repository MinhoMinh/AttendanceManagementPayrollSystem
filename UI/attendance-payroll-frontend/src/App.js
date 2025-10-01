import React, { useState } from "react";
import Header from "./components/Header";
import CheckAttendance from "./components/CheckAttendance";
import ButtonDemo from "./components/ButtonDemo";
import Dashboard from "./components/Dashboard";
import ViewAdjustmentRequests from "./components/ViewAdjustmentRequests";
import "./App.css";

function App() {
  const [currentView, setCurrentView] = useState("dashboard"); // 'dashboard', 'attendance', 'buttons', 'adjustments'

  const handleLogout = () => {
    alert("Đăng xuất thành công!");
    // Thêm logic đăng xuất ở đây
  };

  // return (
  //   <div className="App">
  //     {currentView !== 'dashboard' && (
  //       <Header
  //         title="Attendance Management System"
  //         userName="Nguyễn Văn A"
  //         userRole="Quản lý"
  //         onLogout={handleLogout}
  //         showNotifications={true}
  //         notificationCount={1}
  //       />
  //     )}

  //     <main className="main-content">
  //       {/* Navigation buttons */}
  //       {currentView !== 'dashboard' && (
  //         <div style={{
  //           display: 'flex',
  //           gap: '10px',
  //           marginBottom: '20px',
  //           padding: '0 20px'
  //         }}>
  //           <button
  //             onClick={() => setCurrentView('dashboard')}
  //             style={{
  //               padding: '8px 16px',
  //               backgroundColor: currentView === 'dashboard' ? '#3498db' : '#ecf0f1',
  //               color: currentView === 'dashboard' ? 'white' : '#2c3e50',
  //               border: 'none',
  //               borderRadius: '4px',
  //               cursor: 'pointer'
  //             }}
  //           >
  //             🏠 Dashboard
  //           </button>
  //           <button
  //             onClick={() => setCurrentView('attendance')}
  //             style={{
  //               padding: '8px 16px',
  //               backgroundColor: currentView === 'attendance' ? '#3498db' : '#ecf0f1',
  //               color: currentView === 'attendance' ? 'white' : '#2c3e50',
  //               border: 'none',
  //               borderRadius: '4px',
  //               cursor: 'pointer'
  //             }}
  //           >
  //             📅 Check Attendance
  //           </button>
  //           <button
  //             onClick={() => setCurrentView('buttons')}
  //             style={{
  //               padding: '8px 16px',
  //               backgroundColor: currentView === 'buttons' ? '#3498db' : '#ecf0f1',
  //               color: currentView === 'buttons' ? 'white' : '#2c3e50',
  //               border: 'none',
  //               borderRadius: '4px',
  //               cursor: 'pointer'
  //             }}
  //           >
  //             🎨 Button Demo
  //           </button>
  //           <button
  //             onClick={() => setCurrentView('adjustments')}
  //             style={{
  //               padding: '8px 16px',
  //               backgroundColor: currentView === 'adjustments' ? '#3498db' : '#ecf0f1',
  //               color: currentView === 'adjustments' ? 'white' : '#2c3e50',
  //               border: 'none',
  //               borderRadius: '4px',
  //               cursor: 'pointer'
  //             }}
  //           >
  //             📋 Adjustments
  //           </button>
  //         </div>
  //       )}

  //       {/* Content based on current view */}
  //       {currentView === 'dashboard' && <Dashboard />}
  //       {currentView === 'attendance' && <CheckAttendance />}
  //       {currentView === 'buttons' && <ButtonDemo />}
  //       {currentView === 'adjustments' && <ViewAdjustmentRequests />}
  //     </main>
  //   </div>
  // );
  return (
    <>
    <ViewAdjustmentRequests/>
      <h1>Babaji</h1>
    </>
  );
}

export default App;

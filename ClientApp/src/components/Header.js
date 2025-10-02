import React from 'react';
import './Header.css';

const Header = ({ 
  title = "Attendance Management System", 
  showUserInfo = true, 
  userName = "Admin",
  userRole = "Quản lý",
  onLogout,
  showNotifications = true,
  notificationCount = 0
}) => {
  return (
    <header className="header">
      <div className="header-container">
        {/* Logo và Title */}
        <div className="header-left">
          <div className="logo">
            <span className="logo-icon">📊</span>
            <h1 className="header-title">{title}</h1>
          </div>
        </div>

        {/* Navigation Menu */}
        <nav className="header-nav">
          <ul className="nav-list">
            <li className="nav-item">
              <a href="/dashboard" className="nav-link">Dashboard</a>
            </li>
            <li className="nav-item">
              <a href="/attendance" className="nav-link">Chấm công</a>
            </li>
            <li className="nav-item">
              <a href="/payroll" className="nav-link">Lương</a>
            </li>
            <li className="nav-item">
              <a href="/employees" className="nav-link">Nhân viên</a>
            </li>
            <li className="nav-item">
              <a href="/reports" className="nav-link">Báo cáo</a>
            </li>
          </ul>
        </nav>

        {/* User Info và Actions */}
        <div className="header-right">
          {/* Notifications */}
          {showNotifications && (
            <div className="notification-icon">
              <span className="notification-bell">🔔</span>
              {notificationCount > 0 && (
                <span className="notification-badge">{notificationCount}</span>
              )}
            </div>
          )}

          {/* User Info */}
          {showUserInfo && (
            <div className="user-info">
              <div className="user-avatar">
                <span className="avatar-text">{userName.charAt(0).toUpperCase()}</span>
              </div>
              <div className="user-details">
                <span className="user-name">{userName}</span>
                <span className="user-role">{userRole}</span>
              </div>
              <div className="user-actions">
                <button className="logout-btn" onClick={onLogout}>
                  Đăng xuất
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;

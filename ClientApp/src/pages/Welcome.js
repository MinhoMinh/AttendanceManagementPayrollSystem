import React from "react";
import "../components/Welcome.css";
export default function Welcome({ username, onContinue }) {
  return (
    <div className="welcome-wrapper">
      <div className="welcome-card">
        <img src="/amps-logo.png" alt="Logo" className="welcome-logo" />
        <h1>🎉 Xin chào, {username}!</h1>
        <p>Chào mừng bạn đã đăng nhập vào hệ thống</p>
        <button className="welcome-button" onClick={onContinue}>
          Tiếp tục
        </button>
      </div>
    </div>
  );
}
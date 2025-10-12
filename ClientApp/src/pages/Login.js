import React from "react";
import "../components/Login.css";

export default function Login({ onLogin }) {
  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData(e.target);
    const username = formData.get("username");
    const password = formData.get("password");

    try {
      const response = await fetch("https://localhost:7184/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage || "Đăng nhập thất bại");
      }

      const data = await response.json();

      localStorage.setItem("empId", 3);

      // Lưu token vào localStorage
      if (data.token) {
        localStorage.setItem("token", data.token);
      }

      // Báo cho App biết đã login thành công
      if (onLogin) {
        onLogin(username);
      }
    } catch (err) {
      alert("⚠️ Lỗi đăng nhập: " + err.message);
    }
  };

  return (
    <div className="login-wrapper">
      <div className="card fade-in">
        <img src="/amps-logo.png" alt="AMPS Logo" className="logo fade-in" />
        <h2 className="fade-in">🔑 Đăng nhập</h2>

        <form onSubmit={handleSubmit} className="fade-in">
          <input
            type="text"
            name="username"
            placeholder="Tên đăng nhập"
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Mật khẩu"
            required
          />
          <button type="submit">Đăng nhập</button>
        </form>
      </div>
    </div>
  );
}

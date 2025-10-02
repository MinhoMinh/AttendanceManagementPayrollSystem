import { useState } from "react";
import CustomButton from "./CustomButton";

function ViewAdjustmentRequests() {
  const [requests, setRequests] = useState([
    {
      id: 1,
      requestType: "Time Adjustment",
      date: "2024-01-15",
      description: "Late arrival due to traffic jam",
      status: "Pending",
      employee: "Nguyen Van A"
    },
    {
      id: 2,
      requestType: "Overtime Request",
      date: "2024-01-14",
      description: "Working late on project deadline",
      status: "Approved",
      employee: "Tran Thi B"
    },
    {
      id: 3,
      requestType: "Leave Request",
      date: "2024-01-13",
      description: "Personal emergency leave",
      status: "Rejected",
      employee: "Le Van C"
    },
    {
      id: 4,
      requestType: "Schedule Change",
      date: "2024-01-12",
      description: "Shift swap with colleague",
      status: "Pending",
      employee: "Pham Thi D"
    },
    {
      id: 5,
      requestType: "Break Time Adjustment",
      date: "2024-01-11",
      description: "Extended break for medical appointment",
      status: "Approved",
      employee: "Hoang Van E"
    }
  ]);

  const [filter, setFilter] = useState("All");
  const [searchTerm, setSearchTerm] = useState("");

  const handleApprove = (id) => {
    setRequests(requests.map(req => 
      req.id === id ? { ...req, status: "Approved" } : req
    ));
    alert('Request approved successfully!');
  };

  const handleReject = (id) => {
    setRequests(requests.map(req => 
      req.id === id ? { ...req, status: "Rejected" } : req
    ));
    alert('Request rejected!');
  };

  const getStatusColor = (status) => {
    switch (status) {
      case "Approved": return "#27ae60";
      case "Rejected": return "#e74c3c";
      case "Pending": return "#f39c12";
      default: return "#95a5a6";
    }
  };

  const getStatusIcon = (status) => {
    switch (status) {
      case "Approved": return "✅";
      case "Rejected": return "❌";
      case "Pending": return "⏳";
      default: return "❓";
    }
  };

  const filteredRequests = requests.filter(request => {
    const matchesFilter = filter === "All" || request.status === filter;
    const matchesSearch = request.employee.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         request.requestType.toLowerCase().includes(searchTerm.toLowerCase()) ||
                         request.description.toLowerCase().includes(searchTerm.toLowerCase());
    return matchesFilter && matchesSearch;
  });

  return (
    <div style={{ 
      fontFamily: "Arial, sans-serif", 
      minHeight: "100vh", 
      backgroundColor: "#f8f9fa"
    }}>
      {/* Header */}
      <div
        style={{
          backgroundColor: "#2c3e50",
          color: "white",
          padding: "15px 30px",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          boxShadow: "0 2px 4px rgba(0,0,0,0.1)"
        }}
      >
        <h2 style={{ margin: 0 }}>📋 View Adjustment Requests</h2>
        <div style={{ display: "flex", gap: "10px" }}>
          <button 
            onClick={() => alert('Exporting data...')}
            style={{
              padding: "8px 16px",
              backgroundColor: "#3498db",
              color: "white",
              border: "none",
              borderRadius: "4px",
              cursor: "pointer"
            }}
          >
            Export
          </button>
          <button 
            onClick={() => window.location.reload()}
            style={{
              padding: "8px 16px",
              backgroundColor: "#27ae60",
              color: "white",
              border: "none",
              borderRadius: "4px",
              cursor: "pointer"
            }}
          >
            Refresh
          </button>
        </div>
      </div>

      {/* Main Content */}
      <div style={{ 
        padding: "20px", 
        maxWidth: "1200px", 
        margin: "0 auto"
      }}>
        {/* Filters and Search */}
        <div style={{
          backgroundColor: "white",
          padding: "20px",
          marginBottom: "20px",
          borderRadius: "8px",
          boxShadow: "0 2px 4px rgba(0,0,0,0.1)"
        }}>
          <div style={{ 
            display: "flex", 
            gap: "20px",
            alignItems: "center",
            flexWrap: "wrap"
          }}>
            {/* Search */}
            <div style={{ flex: "1", minWidth: "200px" }}>
              <label style={{ 
                display: "block", 
                marginBottom: "5px", 
                fontWeight: "600", 
                color: "#2c3e50" 
              }}>
                Tìm kiếm
              </label>
              <input
                type="text"
                placeholder="Tìm theo tên, loại yêu cầu..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                style={{
                  width: "100%",
                  padding: "8px 12px",
                  border: "1px solid #ddd",
                  borderRadius: "4px",
                  fontSize: "14px",
                  outline: "none"
                }}
              />
            </div>

            {/* Filter */}
            <div style={{ minWidth: "150px" }}>
              <label style={{ 
                display: "block", 
                marginBottom: "5px", 
                fontWeight: "600", 
                color: "#2c3e50" 
              }}>
                Trạng thái
              </label>
              <select
                value={filter}
                onChange={(e) => setFilter(e.target.value)}
                style={{
                  width: "100%",
                  padding: "8px 12px",
                  border: "1px solid #ddd",
                  borderRadius: "4px",
                  fontSize: "14px",
                  outline: "none",
                  backgroundColor: "white"
                }}
              >
                <option value="All">Tất cả</option>
                <option value="Pending">Đang chờ</option>
                <option value="Approved">Đã duyệt</option>
                <option value="Rejected">Đã từ chối</option>
              </select>
            </div>

            {/* Stats */}
            <div style={{ textAlign: "center", minWidth: "100px" }}>
              <div style={{ fontSize: "1.5rem", fontWeight: "bold", color: "#2c3e50" }}>
                {filteredRequests.length}
              </div>
              <div style={{ fontSize: "0.8rem", color: "#7f8c8d" }}>
                Yêu cầu
              </div>
            </div>
          </div>
        </div>

        {/* Table */}
        <div style={{
          backgroundColor: "white",
          borderRadius: "8px",
          overflow: "hidden",
          boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
          border: "1px solid #ddd"
        }}>
          {/* Table Header */}
          <div style={{
            backgroundColor: "#f8f9fa",
            padding: "15px",
            display: "grid",
            gridTemplateColumns: "1fr 1fr 2fr 1fr 1fr 1fr",
            gap: "15px",
            fontWeight: "600",
            fontSize: "14px",
            borderBottom: "1px solid #ddd"
          }}>
            <div>Request Type</div>
            <div>Date</div>
            <div>Description</div>
            <div>Employee</div>
            <div>Status</div>
            <div>Actions</div>
          </div>

          {/* Table Body */}
          <div>
            {filteredRequests.length === 0 ? (
              <div style={{
                padding: "40px",
                textAlign: "center",
                color: "#7f8c8d",
                fontSize: "1rem"
              }}>
                Không có yêu cầu nào
              </div>
            ) : (
              filteredRequests.map((request, index) => (
                <div
                  key={request.id}
                  style={{
                    padding: "15px",
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr 2fr 1fr 1fr 1fr",
                    gap: "15px",
                    alignItems: "center",
                    borderBottom: index < filteredRequests.length - 1 ? "1px solid #eee" : "none"
                  }}
                >
                  {/* Request Type */}
                  <div style={{ fontWeight: "600", color: "#2c3e50" }}>
                    {request.requestType}
                  </div>

                  {/* Date */}
                  <div style={{ color: "#7f8c8d", fontSize: "0.9rem" }}>
                    {request.date}
                  </div>

                  {/* Description */}
                  <div style={{ color: "#2c3e50", fontSize: "0.9rem" }}>
                    {request.description}
                  </div>

                  {/* Employee */}
                  <div style={{ color: "#2c3e50", fontWeight: "500" }}>
                    {request.employee}
                  </div>

                  {/* Status */}
                  <div style={{
                    color: getStatusColor(request.status),
                    fontWeight: "500"
                  }}>
                    {request.status}
                  </div>

                  {/* Actions */}
                  <div style={{ display: "flex", gap: "5px" }}>
                    {request.status === "Pending" && (
                      <>
                        <button
                          onClick={() => handleApprove(request.id)}
                          style={{
                            padding: "4px 8px",
                            backgroundColor: "#27ae60",
                            color: "white",
                            border: "none",
                            borderRadius: "3px",
                            fontSize: "12px",
                            cursor: "pointer"
                          }}
                        >
                          Approve
                        </button>
                        <button
                          onClick={() => handleReject(request.id)}
                          style={{
                            padding: "4px 8px",
                            backgroundColor: "#e74c3c",
                            color: "white",
                            border: "none",
                            borderRadius: "3px",
                            fontSize: "12px",
                            cursor: "pointer"
                          }}
                        >
                          Reject
                        </button>
                      </>
                    )}
                    {request.status !== "Pending" && (
                      <span style={{ 
                        color: "#7f8c8d", 
                        fontSize: "12px"
                      }}>
                        {request.status === "Approved" ? "Đã xử lý" : "Đã từ chối"}
                      </span>
                    )}
                  </div>
                </div>
              ))
            )}
          </div>
        </div>

        {/* Summary Stats */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(150px, 1fr))",
          gap: "15px",
          marginTop: "20px"
        }}>
          <div style={{
            backgroundColor: "white",
            borderRadius: "8px",
            padding: "15px",
            textAlign: "center",
            boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
            border: "1px solid #ddd"
          }}>
            <div style={{ fontSize: "1.5rem", fontWeight: "bold", color: "#f39c12" }}>
              {requests.filter(r => r.status === "Pending").length}
            </div>
            <div style={{ color: "#7f8c8d", fontSize: "0.8rem" }}>Đang chờ</div>
          </div>

          <div style={{
            backgroundColor: "white",
            borderRadius: "8px",
            padding: "15px",
            textAlign: "center",
            boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
            border: "1px solid #ddd"
          }}>
            <div style={{ fontSize: "1.5rem", fontWeight: "bold", color: "#27ae60" }}>
              {requests.filter(r => r.status === "Approved").length}
            </div>
            <div style={{ color: "#7f8c8d", fontSize: "0.8rem" }}>Đã duyệt</div>
          </div>

          <div style={{
            backgroundColor: "white",
            borderRadius: "8px",
            padding: "15px",
            textAlign: "center",
            boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
            border: "1px solid #ddd"
          }}>
            <div style={{ fontSize: "1.5rem", fontWeight: "bold", color: "#e74c3c" }}>
              {requests.filter(r => r.status === "Rejected").length}
            </div>
            <div style={{ color: "#7f8c8d", fontSize: "0.8rem" }}>Đã từ chối</div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ViewAdjustmentRequests;

import { useEffect, useState } from "react";

function EmployeeAllowancePage() {
    const [allowances, setAllowances] = useState([]);
    const [filtered, setFiltered] = useState([]);
    const [types, setTypes] = useState([]);
    const [selectedType, setSelectedType] = useState("");
    const [showModal, setShowModal] = useState(false);
    const [editItem, setEditItem] = useState(null);
    const [formData, setFormData] = useState({
        empId: "",
        typeId: "",
        customValue: "",
        startDate: "",
        endDate: "",
        status: "Active", // mặc định khi thêm mới
        createdBy: 1, // giả lập người tạo (có thể lấy từ user đăng nhập)
    });

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const res = await fetch("https://localhost:7184/api/employeeallowance/");
            if (!res.ok) throw new Error("Không thể tải dữ liệu");
            const json = await res.json();
            const data = json["$values"] || json;

            setAllowances(data);
            setFiltered(data);

            const distinctTypes = [...new Set(data.map((x) => x.typeName))];
            setTypes(distinctTypes);
        } catch (err) {
            console.error(err);
        }
    };

    // Lọc theo loại phụ cấp
    const handleFilterChange = (e) => {
        const type = e.target.value;
        setSelectedType(type);
        if (!type) setFiltered(allowances);
        else setFiltered(allowances.filter((a) => a.typeName === type));
    };

    // Mở form thêm mới
    const handleAdd = () => {
        setEditItem(null);
        setFormData({
            empId: "",
            typeId: "",
            customValue: "",
            startDate: "",
            endDate: "",
            status: "Active",
            createdBy: 1,
        });
        setShowModal(true);
    };

    // Mở form sửa
    const handleEdit = (item) => {
        setEditItem(item);
        setFormData({
            empId: item.empId,
            typeId: item.typeId,
            customValue: item.customValue ?? "",
            startDate: item.startDate,
            endDate: item.endDate ?? "",
            status: item.status,
            createdBy: item.createdBy,
        });
        setShowModal(true);
    };

    // Gửi dữ liệu
    // Gửi dữ liệu
    const handleSubmit = async (e) => {
        e.preventDefault();
        const method = editItem ? "PUT" : "POST";
        const url = editItem
            ? `https://localhost:7184/api/employeeallowance/${editItem.id}`
            : "https://localhost:7184/api/employeeallowance/";

        const payload = {
            ...formData,
            id: editItem?.id ?? 0, // nếu thêm mới thì id = 0
            customValue:
                formData.customValue === "" ? null : parseFloat(formData.customValue),
            endDate:
                formData.endDate === "" || formData.endDate === null
                    ? null
                    : formData.endDate,
        };

        try {
            const res = await fetch(url, {
                method,
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    id: payload.id,
                    empId: payload.empId,
                    typeId: payload.typeId,
                    customValue: payload.customValue,
                    startDate: payload.startDate,
                    endDate: payload.endDate,
                    status: payload.status,
                    createdBy: payload.createdBy
                }),

            });

            if (!res.ok) {
                const errText = await res.text();
                throw new Error(`Lỗi ${res.status}: ${errText}`);
            }

            console.log(`✅ ${method === "POST" ? "Thêm" : "Cập nhật"} thành công`);
            await fetchData(); // tải lại danh sách
            setShowModal(false); // đóng modal
        } catch (error) {
            console.error("❌ Lỗi khi gửi dữ liệu:", error);
            alert("Có lỗi xảy ra khi lưu dữ liệu. Kiểm tra console để biết chi tiết.");
        }
    };




    // Xử lý nhập liệu
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    return (
        <div className="container mt-4">
            <h3 className="mb-3 text-center">Danh sách phụ cấp nhân viên</h3>

            {/* Bộ lọc + nút thêm */}
            <div className="d-flex justify-content-between align-items-center mb-3">
                <div className="d-flex align-items-center">
                    <label className="me-2 fw-bold">Lọc theo loại phụ cấp:</label>
                    <select
                        className="form-select w-auto"
                        value={selectedType}
                        onChange={handleFilterChange}
                    >
                        <option value="">Tất cả</option>
                        {types.map((type, idx) => (
                            <option key={idx} value={type}>
                                {type}
                            </option>
                        ))}
                    </select>
                </div>

                <button className="btn btn-primary" onClick={handleAdd}>
                    + Thêm mới
                </button>
            </div>

            {/* Bảng danh sách */}
            <table className="table table-bordered table-hover align-middle">
                <thead className="table-light">
                    <tr>
                        <th>#</th>
                        <th>Tên nhân viên</th>
                        <th>Loại phụ cấp</th>
                        <th>Giá trị tùy chỉnh</th>
                        <th>Ngày bắt đầu</th>
                        <th>Ngày kết thúc</th>
                        <th>Trạng thái</th>
                        <th>Người tạo</th>
                        <th>Thao tác</th>
                    </tr>
                </thead>
                <tbody>
                    {filtered.length > 0 ? (
                        filtered.map((item, index) => (
                            <tr key={item.id}>
                                <td>{index + 1}</td>
                                <td>{item.empName}</td>
                                <td>{item.typeName}</td>
                                <td>{item.customValue ?? "-"}</td>
                                <td>{item.startDate}</td>
                                <td>{item.endDate ?? "-"}</td>
                                <td>
                                    <span
                                        className={`badge ${item.status === "Active"
                                            ? "bg-success"
                                            : "bg-secondary"
                                            }`}
                                    >
                                        {item.status}
                                    </span>
                                </td>
                                <td>{item.createdByName ?? item.createdBy}</td>
                                <td>
                                    <button
                                        className="btn btn-sm btn-warning me-2"
                                        onClick={() => handleEdit(item)}
                                    >
                                        Sửa
                                    </button>
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan="9" className="text-center">
                                Không có dữ liệu phù hợp
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

            {/* Modal thêm/sửa */}
            {showModal && (
                <div
                    className="modal fade show d-block"
                    tabIndex="-1"
                    style={{ background: "rgba(0,0,0,0.5)" }}
                >
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">
                                    {editItem ? "Cập nhật phụ cấp" : "Thêm phụ cấp mới"}
                                </h5>
                                <button
                                    type="button"
                                    className="btn-close"
                                    onClick={() => setShowModal(false)}
                                ></button>
                            </div>
                            <div className="modal-body">
                                <form onSubmit={handleSubmit}>
                                    <div className="mb-3">
                                        <label>Mã nhân viên</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            name="empId"
                                            value={formData.empId}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>ID loại phụ cấp</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            name="typeId"
                                            value={formData.typeId}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>Giá trị tùy chỉnh (nếu có)</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            name="customValue"
                                            value={formData.customValue}
                                            onChange={handleChange}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>Ngày bắt đầu</label>
                                        <input
                                            type="date"
                                            className="form-control"
                                            name="startDate"
                                            value={formData.startDate}
                                            onChange={handleChange}
                                            required
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>Ngày kết thúc</label>
                                        <input
                                            type="date"
                                            className="form-control"
                                            name="endDate"
                                            value={formData.endDate}
                                            onChange={handleChange}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label>Trạng thái</label>
                                        <select
                                            className="form-select"
                                            name="status"
                                            value={formData.status}
                                            onChange={handleChange}
                                            required
                                        >
                                            <option value="Active">Active</option>
                                            <option value="Inactive">Inactive</option>
                                        </select>
                                    </div>
                                    <button type="submit" className="btn btn-success w-100">
                                        {editItem ? "Lưu thay đổi" : "Thêm mới"}
                                    </button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default EmployeeAllowancePage;

import { useEffect, useState } from "react";

function AllowanceTypeList() {
    const [allowances, setAllowances] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch("https://localhost:7184/api/allowancetype");
                if (!response.ok) {
                    throw new Error("Không thể tải dữ liệu phụ cấp");
                }
                const data = await response.json();
                console.log("API result:", data);

                // 🩵 Fix: lấy dữ liệu trong $values nếu có
                const list = data?.$values ?? [];
                setAllowances(list);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);


    if (loading) return <p className="text-center mt-4">Đang tải dữ liệu...</p>;
    if (error) return <p className="text-center text-red-600 mt-4">{error}</p>;

    return (
        <div className="container mx-auto p-4">
            <h2 className="text-2xl font-bold mb-4 text-center">Danh sách loại phụ cấp</h2>
            <table className="min-w-full bg-white border border-gray-300 shadow-md rounded-lg">
                <thead className="bg-gray-100">
                    <tr>
                        <th className="py-2 px-4 border-b">#</th>
                        <th className="py-2 px-4 border-b text-left">Tên phụ cấp</th>
                        <th className="py-2 px-4 border-b text-left">Kiểu tính</th>
                        <th className="py-2 px-4 border-b text-right">Giá trị</th>
                        <th className="py-2 px-4 border-b text-center">Ngày hiệu lực</th>
                        <th className="py-2 px-4 border-b text-center">Ngày tạo</th>
                    </tr>
                </thead>
                <tbody>
                    {allowances.map((a, index) => (
                        <tr key={a.typeId} className="hover:bg-gray-50 transition">
                            <td className="py-2 px-4 border-b text-center">{index + 1}</td>
                            <td className="py-2 px-4 border-b">{a.typeName}</td>
                            <td className="py-2 px-4 border-b">{a.calculationType}</td>
                            <td className="py-2 px-4 border-b text-right">{a.value?.toLocaleString("vi-VN")}</td>
                            <td className="py-2 px-4 border-b text-center">
                                {a.effectiveStartDate ? new Date(a.effectiveStartDate).toLocaleDateString("vi-VN") : "-"}
                            </td>
                            <td className="py-2 px-4 border-b text-center">
                                {a.createdAt ? new Date(a.createdAt).toLocaleString("vi-VN") : "-"}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default AllowanceTypeList;

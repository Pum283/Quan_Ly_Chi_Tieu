# Danh sách chức năng (phân nhóm)

Tài liệu này tổng hợp từ `yeu-cau-chuc-nang-web-quan-ly-chi-tieu.md`, mục tiêu là liệt kê đầy đủ chức năng theo nhóm để tiện theo dõi và lập kế hoạch triển khai.

## Nguyên tắc phạm vi nghiệp vụ

- Sản phẩm là hệ thống **quản lý dòng tiền** (money flow management), **không phải** hệ thống lưu ký/giữ tiền.
- Không triển khai nghiệp vụ giữ hộ tài sản, ví thanh toán nội bộ, hoặc trung gian thanh toán.
- Các chức năng số dư, ví/quỹ, chuyển quỹ trong tài liệu là nghiệp vụ ghi nhận và đối soát dữ liệu quản trị.

## Nguyên tắc cấu hình lựa chọn (preset + custom)

- Với mọi dữ liệu cần chọn/phân loại, hệ thống luôn có 2 nguồn:
  - Bộ lựa chọn mặc định do hệ thống cung cấp.
  - Bộ lựa chọn do người dùng tự custom (thêm/sửa/ẩn) theo workspace.
- Áp dụng xuyên suốt cho các chức năng, không chỉ riêng thu nhập.
- Mục tiêu: vừa có chuẩn dùng ngay, vừa linh hoạt theo nhu cầu thực tế từng người dùng/nhóm.

## 1) Nhóm tài khoản và truy cập

- Đăng ký, đăng nhập bằng email.
- Đăng nhập SSO (Google/Microsoft).
- Quên mật khẩu, đặt lại mật khẩu.
- Xác thực hai lớp (2FA) cho vai trò quản trị.
- Quản lý phiên đăng nhập, đăng xuất tất cả thiết bị.
- Quản lý vai trò: Admin (hệ thống), Owner, Member, Viewer (tùy chọn).
- Kiểm soát truy cập theo vai trò (RBAC).

## 2) Nhóm workspace và cộng tác

- Tạo nhiều workspace trong một tài khoản.
- Chọn loại workspace: cá nhân, gia đình, nhóm/tổ chức nhỏ.
- Mời thành viên qua email hoặc link mời.
- Thiết lập tiền tệ mặc định cho workspace.
- Bật/tắt mức chia sẻ dữ liệu cá nhân trong nhóm.
- Chủ workspace xem tổng hợp và drill-down theo từng thành viên.
- Thành viên có trang tổng quan cá nhân riêng.

## 3) Nhóm ví/quỹ và nguồn tiền

- Tạo nguồn tiền: tiền mặt, ngân hàng, ví điện tử, quỹ nhóm.
- Cập nhật số dư đầu kỳ.
- Chuyển tiền giữa các ví/quỹ.
- Khóa ví để ngừng phát sinh giao dịch mới.
- Đối soát số dư đầu kỳ - phát sinh - cuối kỳ.

## 4) Nhóm danh mục và phân loại

- Quản lý danh mục thu/chi dạng cây nhiều cấp (cha/con).
- Cung cấp danh mục mặc định.
- Thêm, sửa, ẩn danh mục theo nhu cầu.
- Gắn màu, icon, từ khóa để lọc nhanh.
- Tự gợi ý danh mục dựa trên lịch sử giao dịch (rule engine).
- Phân loại nguồn thu nhập bằng danh mục mặc định hoặc danh mục tự định nghĩa.

## 5) Nhóm giao dịch thu/chi/chuyển khoản

- Tạo giao dịch với thông tin:
  - Loại giao dịch (thu/chi/chuyển khoản/công nợ).
  - Số tiền.
  - Ví nguồn/đích.
  - Danh mục.
  - Người tạo, người chịu chi phí.
  - Ngày giao dịch, ghi chú.
  - Tệp đính kèm (hóa đơn/chứng từ).
- Khi nhập số tiền, người dùng chọn được phân loại từ bộ hệ thống hoặc bộ custom.
- Lưu mẫu giao dịch (template) để nhập nhanh cho các khoản lặp thường gặp.
- Chỉnh sửa, xóa giao dịch theo quyền.
- Soft delete và khôi phục giao dịch đã xóa nhầm.
- Lưu lịch sử thay đổi (audit log).
- Tìm kiếm giao dịch theo từ khóa, số tiền, mã giao dịch.
- Lọc nâng cao nhiều điều kiện.
- Phát hiện giao dịch trùng hoặc bất thường (rule-based).

## 6) Nhóm giao dịch lặp và tự động hóa

- Thiết lập giao dịch lặp: ngày/tuần/tháng/quý/năm.
- Cấu hình ngày kết thúc hoặc không giới hạn.
- Nhắc giao dịch lặp sắp phát sinh.
- Tự áp tag theo từ khóa/nhà cung cấp.
- Tự tạo giao dịch mẫu cho hành vi lặp không cố định ngày.

## 7) Nhóm công nợ và hoàn trả

- Theo dõi phải thu (người vay bạn).
- Theo dõi phải trả (bạn vay người khác).
- Tạo lịch nhắc thanh toán theo hạn.
- Ghi nhận thanh toán từng phần hoặc toàn phần.
- Cập nhật trạng thái công nợ:
  - Chưa đến hạn.
  - Đến hạn.
  - Quá hạn.
  - Hoàn tất.

## 8) Nhóm ngân sách

- Tạo ngân sách theo danh mục, ví, dự án/nhóm, kỳ tháng/quý/năm.
- Đặt ngưỡng cảnh báo (80%, 100%, vượt ngân sách).
- Theo dõi dự toán, thực chi, chênh lệch.
- Cảnh báo vượt ngân sách.
- Cảnh báo ngưỡng ngân sách cá nhân hóa theo danh mục/ví/mục tiêu.

## 9) Nhóm dòng tiền, dự báo và mô phỏng

- Tính dòng tiền thuần theo tuần/tháng/quý.
- Hiển thị số dư dự kiến từ giao dịch lặp + công nợ đến hạn.
- Cảnh báo thiếu hụt tiền mặt sắp tới.
- Dự báo dòng tiền nâng cao (giai đoạn mở rộng).
- Tạo kịch bản tài chính mô phỏng khi tăng/giảm nhóm chi phí.

## 10) Nhóm quy trình duyệt giao dịch

- Bật/tắt duyệt giao dịch (mặc định tắt).
- Duyệt 1 cấp theo ngưỡng do Owner cấu hình.
- Quản lý trạng thái:
  - Nháp.
  - Chờ duyệt.
  - Đã duyệt.
  - Từ chối.
  - Hủy.
- Ghi chú lý do duyệt/từ chối để minh bạch.
- Thông báo cho người duyệt khi có giao dịch chờ duyệt.

## 11) Nhóm dashboard, báo cáo và KPI

- Dashboard tổng quan: tổng thu, tổng chi, dòng tiền thuần, số dư hiện tại.
- Biểu đồ:
  - Cơ cấu chi tiêu theo danh mục.
  - Xu hướng thu/chi theo thời gian.
  - So sánh kế hoạch và thực tế.
- Bộ lọc báo cáo: thời gian, thành viên, ví, danh mục, dự án.
- Xuất dữ liệu: CSV, Excel, PDF.
- KPI tài chính:
  - Tỷ lệ tiết kiệm.
  - Tỷ lệ chi phí cố định/tổng chi.
  - Tỷ lệ nợ quá hạn.
  - Tỷ lệ hoàn thành ngân sách theo danh mục.
  - Dòng tiền thuần trung bình theo tháng.
- Insight cá nhân hóa theo mục tiêu tài chính.
- Phân tích xu hướng chi tiêu theo chu kỳ cá nhân (kỳ lương, mùa vụ, giai đoạn cao điểm).

## 12) Nhóm cá nhân hóa trải nghiệm

- Thiết lập phong cách quản lý tài chính (chặt chẽ/cân bằng/linh hoạt).
- Thiết lập ưu tiên tài chính (tiết kiệm, đầu tư, nợ, quỹ dự phòng).
- Khai báo chu kỳ thu nhập theo thực tế (tháng/dự án/mùa vụ).
- Tùy chọn widget dashboard theo nhu cầu.
- Sắp xếp thứ tự thông tin ưu tiên.
- Lưu nhiều layout dashboard theo ngữ cảnh.
- Tùy chỉnh kênh nhận nhắc và tần suất nhắc.
- Tùy chỉnh độ nhạy cảnh báo bất thường chi tiêu.

## 13) Nhóm thông báo và nhắc việc

- Gửi thông báo in-app.
- Gửi thông báo qua email.
- Nhắc đến hạn công nợ.
- Nhắc vượt ngân sách.
- Nhắc giao dịch cần duyệt.
- Nhắc giao dịch lặp sắp phát sinh.

## 14) Nhóm quản trị hệ thống (mở rộng)

- Quản lý gói dịch vụ (free/pro).
- Quản lý dung lượng lưu trữ chứng từ.
- Nhật ký hoạt động toàn hệ thống.
- Cấu hình giới hạn tính năng theo gói.
- Khóa kỳ dữ liệu (period lock) theo tháng/quý và cơ chế mở khóa có kiểm soát.

## 15) Nhóm tích hợp và mở rộng

- Kết nối ngân hàng.
- Đồng bộ ví điện tử.
- Gợi ý tối ưu chi tiêu bằng AI.
- Ứng dụng mobile đồng bộ thời gian thực.
- Import dữ liệu lịch sử từ CSV/Excel.
- Webhook/Event export để tích hợp hệ thống ngoài (BI, thông báo, automation).

## 16) Nhóm chức năng theo giai đoạn triển khai

### MVP (bắt buộc)

- Đăng nhập/đăng ký.
- Workspace cá nhân + nhóm.
- Quản lý ví và danh mục.
- Nhập giao dịch thu/chi/chuyển.
- Dashboard cơ bản.
- Báo cáo thu/chi theo tháng.
- Nhắc nợ đơn giản.
- Cá nhân hóa cơ bản (chọn widget dashboard, ngưỡng cảnh báo theo danh mục).
- Mẫu giao dịch cơ bản để nhập nhanh.
- Import dữ liệu cơ bản từ CSV/Excel.
- Soft delete và khôi phục cho giao dịch.

### V2

- Duyệt giao dịch nhiều cấp.
- Ngân sách nâng cao và cảnh báo linh hoạt.
- Công nợ chi tiết + lịch sử thanh toán.
- Xuất báo cáo PDF/Excel đầy đủ.
- Rule tự động hóa (gợi ý danh mục, auto-tag).
- Nhắc việc cá nhân hóa nâng cao.
- Merge danh mục trùng và tự động remap dữ liệu giao dịch.
- Khóa kỳ dữ liệu theo tháng/quý.
- Rule cảnh báo tùy biến theo điều kiện người dùng thiết lập.
- Audit log chi tiết cấp bản ghi (giá trị trước/sau, người thay đổi).

### V3

- Tích hợp ngân hàng/ví điện tử.
- Dự báo dòng tiền nâng cao.
- AI gợi ý tối ưu chi tiêu.
- Ứng dụng mobile đồng bộ thời gian thực.
- Insight và mô phỏng tài chính cá nhân hóa theo mục tiêu dài hạn.
- Webhook/Event export nâng cao cho tích hợp hệ sinh thái ngoài.
- Phân quyền nâng cao theo ví/dự án (tùy chọn enterprise).

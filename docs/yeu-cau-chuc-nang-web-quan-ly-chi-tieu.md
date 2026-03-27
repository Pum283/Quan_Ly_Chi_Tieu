# Đặc tả yêu cầu và chức năng web quản lý chi tiêu, doanh thu, dòng tiền

## 1) Mục tiêu sản phẩm

Xây dựng một ứng dụng web giúp:

- Quản lý tài chính **cá nhân** hoặc **nhóm/tập thể** trên cùng một nền tảng.
- Theo dõi đầy đủ **thu nhập (doanh thu)**, **chi tiêu**, **dòng tiền**, **công nợ**, **ngân sách**.
- Cung cấp báo cáo trực quan để hỗ trợ ra quyết định tài chính ngắn hạn và dài hạn.
- Chuẩn hóa quy trình nhập liệu, kiểm soát duyệt chi, và minh bạch lịch sử giao dịch.
- Hỗ trợ **cá nhân hóa sâu theo từng người dùng** để phù hợp thói quen tài chính, mức kỷ luật chi tiêu, và mục tiêu riêng.

### 1.1 Nguyên tắc nghiệp vụ cốt lõi (phi lưu ký tiền)

- Hệ thống **không trực tiếp lưu giữ tiền** của người dùng hoặc tổ chức.
- Hệ thống chỉ đóng vai trò **ghi nhận, quản lý, theo dõi và phân tích dòng tiền**.
- Hệ thống **không** thực hiện chức năng ví thanh toán, trung gian thanh toán, hoặc giữ hộ tài sản.
- Mọi số dư trong hệ thống là **số liệu quản trị nội bộ**, phục vụ đối soát và ra quyết định.
- Nếu tích hợp ngân hàng/ví điện tử, hệ thống chỉ đồng bộ dữ liệu giao dịch và số dư tham chiếu, không nắm giữ quyền sở hữu tiền.

### 1.2 Nguyên tắc cấu hình danh mục và lựa chọn dữ liệu

- Với các dữ liệu cần phân loại (ví dụ: nguồn thu nhập, loại chi phí, ví/quỹ, thẻ/tag, mục tiêu), hệ thống phải hỗ trợ:
  - Bộ lựa chọn mặc định do hệ thống cung cấp.
  - Người dùng tự thêm/sửa/ẩn mục tùy nhu cầu.
- Khi người dùng nhập một khoản tiền (thu hoặc chi), bắt buộc có khả năng gắn phân loại tương ứng.
- Cùng một nguyên tắc “preset + custom” được áp dụng nhất quán cho các chức năng liên quan trong toàn hệ thống.
- Dữ liệu mặc định cần cho phép mở rộng theo workspace để phù hợp cá nhân/nhóm.

## 2) Phạm vi nghiệp vụ

### 2.1 Mô hình sử dụng

- **Cá nhân**: quản lý thu/chi hàng ngày, tiết kiệm, mục tiêu tài chính.
- **Gia đình/nhóm bạn**: chia sẻ ví, đóng góp quỹ chung, ghi nhận chi hộ và hoàn trả.
- **Nhóm làm việc/CLB nhỏ**: theo dõi doanh thu từ hoạt động, quản lý chi phí vận hành, đối soát quỹ.

### 2.2 Loại giao dịch chính

- Thu nhập định kỳ (lương, lợi nhuận, hoa hồng).
- Thu nhập không định kỳ (thưởng, bán tài sản, hoàn tiền).
- Chi phí cố định (tiền nhà, điện nước, internet).
- Chi phí biến đổi (ăn uống, di chuyển, giải trí).
- Giao dịch chuyển quỹ nội bộ (ví A sang ví B).
- Công nợ phải thu/phải trả.

## 3) Vai trò người dùng và quyền hạn

## 3.1 Vai trò mặc định

- **Quản trị hệ thống (Admin)**:
  - Quản trị cấu hình hệ thống ở mức platform.
  - Quản lý gói dịch vụ, giới hạn tính năng, và giám sát vận hành toàn hệ thống.
  - Không thay thế vai trò Owner trong vận hành nghiệp vụ từng workspace.
- **Chủ (Owner)**:
  - Tạo workspace, mời thành viên, chỉnh cài đặt chung.
  - Xem toàn bộ dữ liệu trong workspace.
  - Có thể bật/tắt yêu cầu duyệt chi tiêu (nếu cần).
- **Thành viên (Member)**:
  - Tạo/sửa giao dịch của mình.
  - Xem dashboard và báo cáo chung theo phạm vi được chia sẻ.
  - Tạo yêu cầu hoàn ứng hoặc ghi nhận chi hộ.
- **Chỉ xem (Viewer) - tùy chọn**:
  - Chỉ xem tổng quan và báo cáo.
  - Không tạo/sửa/xóa giao dịch.

### 3.2 Nguyên tắc phân quyền

- Ưu tiên mô hình quyền **đơn giản, dễ hiểu** như app mobile.
- Mặc định ở phạm vi workspace có 2 vai trò chính: **Owner** và **Member**.
- Bổ sung vai trò **Admin** ở mức hệ thống để quản trị platform và vận hành toàn cục.
- Chỉ dùng **Viewer** khi thật sự cần chia sẻ báo cáo cho người không nhập liệu.
- Hạn chế cấu hình quyền theo module chi tiết để tránh khó dùng.

## 4) Yêu cầu chức năng chi tiết

## 4.1 Quản lý tài khoản và xác thực

- Đăng ký/đăng nhập bằng email hoặc tài khoản SSO (Google/Microsoft).
- Quên mật khẩu, đặt lại mật khẩu an toàn.
- Xác thực hai lớp (2FA) cho vai trò quản trị.
- Quản lý phiên đăng nhập, đăng xuất khỏi tất cả thiết bị.

## 4.2 Quản lý workspace (cá nhân/tập thể)

- Tạo nhiều workspace trong một tài khoản.
- Chọn loại workspace:
  - Cá nhân.
  - Gia đình.
  - Nhóm/tổ chức nhỏ.
- Mời thành viên qua email/link mời.
- Thiết lập đơn vị tiền tệ mặc định cho workspace.

## 4.3 Quản lý ví/quỹ/tài khoản tiền

- Tạo nhiều “nguồn tiền”:
  - Tiền mặt.
  - Tài khoản ngân hàng.
  - Ví điện tử.
  - Quỹ nhóm.
- Cập nhật số dư đầu kỳ.
- Chuyển tiền giữa các ví.
- Khóa ví (không cho phát sinh giao dịch mới).

## 4.4 Quản lý danh mục thu/chi

- Danh mục theo cây nhiều cấp (cha/con).
- Mặc định có bộ danh mục chuẩn.
- Cho phép thêm/sửa/ẩn danh mục tùy nhu cầu.
- Gắn màu, icon, từ khóa để lọc nhanh.
- Hỗ trợ phân loại nguồn thu nhập theo cả danh mục hệ thống và danh mục tự định nghĩa của người dùng.

## 4.5 Quản lý giao dịch thu/chi

- Tạo giao dịch với các trường:
  - Loại giao dịch (thu/chi/chuyển khoản/công nợ).
  - Số tiền.
  - Ví nguồn/đích.
  - Danh mục.
  - Người tạo, người chịu chi phí.
  - Ngày giao dịch, ghi chú.
  - Tệp đính kèm (hóa đơn/chứng từ).
- Khi nhập khoản thu/chi, người dùng phải chọn được phân loại từ danh mục mặc định hoặc danh mục tự tạo.
- Hỗ trợ giao dịch lặp:
  - Hàng ngày/tuần/tháng/quý/năm.
  - Có ngày kết thúc hoặc không.
- Chỉnh sửa/xóa giao dịch theo quyền.
- Lưu lịch sử thay đổi (audit log).

## 4.6 Quản lý công nợ và hoàn trả

- Theo dõi:
  - Người vay bạn.
  - Bạn vay người khác.
- Tạo lịch nhắc thanh toán theo hạn.
- Ghi nhận thanh toán từng phần.
- Tự động cập nhật trạng thái công nợ:
  - Chưa đến hạn.
  - Đến hạn.
  - Quá hạn.
  - Hoàn tất.

## 4.7 Quản lý ngân sách

- Tạo ngân sách theo:
  - Danh mục.
  - Ví.
  - Dự án/nhóm.
  - Tháng/quý/năm.
- Đặt ngưỡng cảnh báo (ví dụ 80%, 100%, vượt ngân sách).
- Theo dõi:
  - Dự toán.
  - Thực chi.
  - Chênh lệch.

## 4.8 Dòng tiền và dự báo

- Tính dòng tiền thuần theo kỳ:
  - Tuần.
  - Tháng.
  - Quý.
- Hiển thị “số dư dự kiến” theo giao dịch lặp + công nợ đến hạn.
- Cảnh báo thiếu hụt tiền mặt sắp tới.

## 4.9 Quy trình duyệt giao dịch (cho tập thể)

- Tùy chọn bật/tắt duyệt giao dịch (mặc định tắt để thao tác nhanh).
- Nếu bật duyệt:
  - Chỉ 1 cấp duyệt theo ngưỡng do Owner cấu hình.
- Trạng thái giao dịch:
  - Nháp.
  - Chờ duyệt.
  - Đã duyệt.
  - Từ chối.
  - Hủy.
- Có ghi chú duyệt/từ chối để minh bạch.

## 4.10 Dashboard và báo cáo

- Dashboard tổng quan:
  - Tổng thu.
  - Tổng chi.
  - Dòng tiền thuần.
  - Số dư hiện tại.
- Biểu đồ:
  - Cơ cấu chi tiêu theo danh mục.
  - Xu hướng thu/chi theo thời gian.
  - So sánh kế hoạch và thực tế.
- Bộ lọc nâng cao:
  - Theo thời gian.
  - Theo thành viên.
  - Theo ví.
  - Theo danh mục.
  - Theo dự án.
- Xuất dữ liệu:
  - CSV.
  - Excel.
  - PDF báo cáo.

## 4.11 Tìm kiếm, lọc, và đối soát

- Tìm kiếm giao dịch theo từ khóa, số tiền, mã giao dịch.
- Lọc nâng cao nhiều điều kiện.
- Đối soát số dư đầu kỳ - phát sinh - cuối kỳ.
- Phát hiện giao dịch trùng hoặc bất thường (rule-based).

## 4.12 Thông báo và nhắc việc

- Kênh thông báo:
  - In-app.
  - Email.
- Nội dung nhắc:
  - Đến hạn công nợ.
  - Vượt ngân sách.
  - Giao dịch cần duyệt.
  - Giao dịch lặp sắp phát sinh.

## 4.13 Quản trị hệ thống (giai đoạn mở rộng)

- Quản lý gói dịch vụ (free/pro).
- Quản lý dung lượng lưu trữ chứng từ.
- Nhật ký hoạt động toàn hệ thống.
- Cấu hình giới hạn tính năng theo gói.

## 4.14 Cá nhân hóa theo nhu cầu từng người dùng

- Hồ sơ tài chính cá nhân hóa:
  - Thiết lập phong cách quản lý (chặt chẽ/cân bằng/linh hoạt).
  - Thiết lập ưu tiên tài chính (tiết kiệm, đầu tư, kiểm soát nợ, duy trì quỹ dự phòng).
  - Khai báo chu kỳ thu nhập thực tế (theo tháng, theo dự án, theo mùa vụ).
- Dashboard cá nhân hóa:
  - Cho phép chọn widget hiển thị (dòng tiền, nợ đến hạn, danh mục chi vượt mức, mục tiêu).
  - Sắp xếp thứ tự ưu tiên thông tin theo người dùng.
  - Lưu nhiều layout dashboard theo ngữ cảnh (cá nhân, gia đình, công việc).
- Cảnh báo và nhắc việc cá nhân hóa:
  - Cấu hình ngưỡng cảnh báo riêng theo danh mục/ví/mục tiêu.
  - Chọn kênh nhận nhắc và tần suất nhắc theo từng loại sự kiện.
  - Tùy chỉnh mức độ nhạy cảnh báo bất thường chi tiêu.
- Quy tắc tự động hóa (rule engine):
  - Tự gợi ý danh mục dựa trên lịch sử giao dịch.
  - Tự áp thẻ (tag) cho giao dịch theo từ khóa/nhà cung cấp.
  - Tự tạo giao dịch mẫu cho hành vi lặp có chu kỳ nhưng không cố định ngày.
- Báo cáo và insight cá nhân hóa:
  - Đề xuất chỉ số ưu tiên theo mục tiêu từng người (ví dụ tiết kiệm nhanh, giảm nợ).
  - So sánh hành vi chi tiêu theo giai đoạn cá nhân (tháng lương, kỳ cao điểm công việc).
  - Tạo “kịch bản tài chính” để mô phỏng nếu tăng/giảm một nhóm chi phí.
- Trải nghiệm theo vai trò trong workspace:
  - Mỗi thành viên có trang tổng quan cá nhân riêng.
  - Chủ workspace có chế độ xem tổng hợp + drill-down theo từng thành viên.
  - Cho phép bật/tắt mức chia sẻ dữ liệu cá nhân trong nhóm.

## 5) Yêu cầu phi chức năng

## 5.1 Hiệu năng

- Thời gian tải dashboard chính < 3 giây với dữ liệu chuẩn.
- Truy vấn danh sách giao dịch phân trang ổn định.
- Hỗ trợ tối thiểu 1.000.000 giao dịch/workspace lớn (khi mở rộng).

## 5.2 Bảo mật

- Mã hóa dữ liệu nhạy cảm.
- Mật khẩu băm an toàn.
- Chống CSRF/XSS/SQL Injection.
- Kiểm soát truy cập theo vai trò (RBAC).
- Audit log cho thao tác quan trọng.

## 5.3 Độ tin cậy và sao lưu

- Cơ chế backup định kỳ.
- Khả năng khôi phục dữ liệu khi có sự cố.
- Hạn chế mất dữ liệu giao dịch (RPO thấp).

## 5.4 Khả dụng

- Thiết kế theo hướng **mobile app experience trên web** (mobile-first).
- Tối ưu thao tác một tay, nút bấm lớn, vùng chạm rõ ràng.
- Điều hướng dạng bottom navigation cho màn hình nhỏ.
- Hỗ trợ nhập nhanh trong 1-2 bước như ứng dụng điện thoại.
- Thiết kế responsive cho desktop/tablet/mobile.
- Hỗ trợ trình duyệt hiện đại (Chrome, Edge, Firefox, Safari).
- Giao diện dễ sử dụng cho người không chuyên tài chính.

## 5.5 Khả năng mở rộng

- Thiết kế module hóa để mở rộng:
  - Kết nối ngân hàng.
  - Đồng bộ ví điện tử.
  - AI gợi ý tối ưu chi tiêu.

## 6) Dữ liệu cốt lõi (mô hình logic)

Các thực thể chính cần có:

- User
- Workspace
- Membership (vai trò trong workspace)
- Wallet (nguồn tiền/quỹ)
- Category (danh mục)
- Transaction
- RecurringTransaction
- Budget
- Debt
- ApprovalFlow
- Notification
- Attachment (hóa đơn/chứng từ)
- AuditLog

## 7) Luồng nghiệp vụ quan trọng

## 7.1 Luồng ghi nhận chi tiêu cá nhân

1. Người dùng chọn workspace cá nhân.
2. Tạo giao dịch chi với danh mục và ví.
3. Hệ thống trừ số dư ví và cập nhật dashboard.
4. Nếu vượt ngưỡng ngân sách, sinh cảnh báo.

## 7.2 Luồng ghi nhận chi tiêu tập thể có duyệt

1. Thành viên tạo giao dịch ở trạng thái “Chờ duyệt”.
2. Người duyệt nhận thông báo.
3. Duyệt hoặc từ chối kèm lý do.
4. Nếu duyệt, hệ thống mới ghi nhận phát sinh vào sổ quỹ.

## 7.3 Luồng công nợ và thanh toán

1. Tạo khoản nợ (phải thu/phải trả) và hạn thanh toán.
2. Hệ thống gửi nhắc trước hạn.
3. Ghi nhận thanh toán một phần hoặc toàn phần.
4. Cập nhật trạng thái công nợ tương ứng.

## 8) Bộ KPI theo dõi tài chính

- Tỷ lệ tiết kiệm = (Thu - Chi) / Thu.
- Tỷ lệ chi phí cố định / tổng chi.
- Tỷ lệ nợ quá hạn.
- Tỷ lệ hoàn thành ngân sách theo danh mục.
- Dòng tiền thuần trung bình theo tháng.

## 9) Backlog chức năng theo giai đoạn

## 9.1 Giai đoạn MVP (bắt buộc)

- Đăng nhập/đăng ký.
- Workspace cá nhân + nhóm.
- Quản lý ví và danh mục.
- Nhập giao dịch thu/chi/chuyển.
- Dashboard cơ bản.
- Báo cáo thu/chi theo tháng.
- Nhắc nợ đơn giản.
- Cá nhân hóa cơ bản: chọn widget dashboard và cấu hình ngưỡng cảnh báo theo danh mục.

## 9.2 Giai đoạn V2

- Duyệt giao dịch nhiều cấp.
- Ngân sách nâng cao và cảnh báo linh hoạt.
- Công nợ chi tiết + lịch sử thanh toán.
- Xuất báo cáo PDF/Excel đầy đủ.
- Rule tự động hóa (gợi ý danh mục, auto-tag) và nhắc việc cá nhân hóa nâng cao.

## 9.3 Giai đoạn V3

- Tích hợp ngân hàng/ví điện tử.
- Dự báo dòng tiền nâng cao.
- Gợi ý tối ưu chi tiêu bằng AI.
- Ứng dụng mobile đồng bộ thời gian thực.
- Insight và mô phỏng tài chính cá nhân hóa theo mục tiêu dài hạn.

## 10) Tiêu chí nghiệm thu

- Người dùng có thể ghi nhận giao dịch trong < 10 giây thao tác.
- Dashboard hiển thị đúng tổng thu/chi/số dư theo bộ lọc.
- Quy trình duyệt hoạt động đúng theo ngưỡng cấu hình.
- Dữ liệu xuất báo cáo khớp với dữ liệu màn hình.
- Quyền truy cập không bị rò rỉ giữa các workspace.

## 11) Rủi ro và đề xuất kiểm soát

- **Rủi ro nhập liệu sai**: thêm quy tắc validate + gợi ý danh mục thông minh.
- **Rủi ro thiếu minh bạch nhóm**: bắt buộc chứng từ cho giao dịch vượt ngưỡng.
- **Rủi ro quá tải dữ liệu**: triển khai phân trang + indexing + archive.
- **Rủi ro bảo mật**: 2FA, giới hạn phiên, audit log, cảnh báo đăng nhập lạ.
- **Rủi ro cá nhân hóa sai trọng tâm**: cần onboarding hỏi mục tiêu và cho phép người dùng chỉnh profile tài chính bất kỳ lúc nào.

## 12) Định hướng UI/UX (đề xuất)

- Trải nghiệm ưu tiên giống app điện thoại:
  - Màn hình chính hiển thị số dư, thu/chi tháng, cảnh báo nổi bật.
  - Nút “+” cố định để thêm giao dịch nhanh ở mọi màn hình.
  - Quy trình nhập giao dịch tối đa 2 bước.
- Điều hướng chính gọn nhẹ:
  - Tổng quan.
  - Giao dịch.
  - Ngân sách.
  - Báo cáo.
  - Cài đặt.
- Mẫu nhập liệu thông minh:
  - Gợi ý danh mục theo lịch sử.
  - Ghi nhớ ví và danh mục dùng gần nhất.
  - Tự điền nội dung lặp thường xuyên.
- Cung cấp onboarding ngắn 3-5 bước để khởi tạo nhanh, có thể dùng ngay.

---

## Phụ lục A - Danh sách user stories mẫu

- Là người dùng cá nhân, tôi muốn thêm khoản chi trong 5 giây để không quên giao dịch.
- Là quản trị nhóm, tôi muốn duyệt chi phí vượt ngưỡng để kiểm soát quỹ.
- Là thành viên, tôi muốn xem số tiền mình đã tạm ứng để biết khi nào được hoàn trả.
- Là owner, tôi muốn xem báo cáo dòng tiền theo tháng để đánh giá sức khỏe tài chính.

## Phụ lục B - Đề xuất menu chức năng

- Tổng quan
- Giao dịch
- Ví/Quỹ
- Danh mục
- Ngân sách
- Công nợ
- Báo cáo
- Thành viên & phân quyền
- Cài đặt

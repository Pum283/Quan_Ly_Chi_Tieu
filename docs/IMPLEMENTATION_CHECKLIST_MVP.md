# IMPLEMENTATION CHECKLIST (MVP)

Checklist này dùng để triển khai dự án quản lý dòng tiền từ 0 -> chạy được bản MVP ổn định.

Phạm vi áp dụng:
- BE: .NET Clean Architecture (`API`, `Application`, `Domain`, `Infrastructure`)
- FE: ReactJS base
- Nghiệp vụ: quản lý dòng tiền (non-custodial), không giữ tiền trực tiếp

---

## 0) Chốt scope trước khi code

- [x] Xác nhận scope triển khai: **toàn bộ thiết kế đã thống nhất trong bộ tài liệu hiện tại**.
- [x] Không chia nhỏ MVP/V2/V3 ở bước chốt scope ban đầu, triển khai theo full thiết kế.
- [x] Chốt nguyên tắc nghiệp vụ: non-custodial + preset/custom cho dữ liệu phân loại.
- [x] Chốt vai trò MVP: Owner, Member, Admin (Viewer optional).
- [x] Chốt luồng chính cần demo: đăng nhập -> workspace -> ví/danh mục -> nhập giao dịch -> dashboard.

---

## 1) Chuẩn bị môi trường và cấu hình

### 1.1 Backend environment
- [x] Tạo file env theo môi trường (dev/staging/prod).
- [x] Khai báo đầy đủ biến quan trọng:
  - [x] DB connection string
  - [x] JWT signing key
  - [x] SMTP credentials (nếu dùng)
  - [x] CORS origin
- [x] Đảm bảo không lưu secrets trong `appsettings.json`.
- [x] Cập nhật tài liệu env runtime (`.env`) và danh sách biến bắt buộc.

### 1.2 Frontend environment
- [x] Khai báo `API_BASE_URL`.
- [x] Khai báo feature flags cơ bản cho MVP.
- [x] Kiểm tra cấu hình build/dev phù hợp local team.

### 1.3 Tooling
- [x] Chạy build BE thành công.
- [x] Chạy build FE thành công.
- [x] Cấu hình lint/format/test scripts.
- [x] Chốt version runtime (dotnet/node) cho toàn team.

---

## 2) Thiết kế dữ liệu và migration plan

- [x] Rà lại `thiet-ke-he-thong-database.md` với team.
- [x] Chốt enum và trạng thái nghiệp vụ trước khi tạo migration.
- [x] Chia migration theo pha:
  - [x] Pha 1: core tables (auth/workspace/wallet/category/transaction/debt/budget)
  - [x] Pha 2: approval/audit/period lock/templates
  - [x] Pha 3: personalization/rules/snapshots
- [x] Tạo index bắt buộc cho bảng `Transactions`, `Debts`, `Budgets`, `AuditLogs`.
- [x] Kiểm tra unique/check constraints quan trọng.
- [x] Chốt migration local từ DB rỗng theo command chuẩn trong `docs/DATA_MIGRATION_PLAN.md`.
- [x] Chốt seed data tối thiểu (workspace, category preset, wallet mẫu).

---

## 3) Triển khai BE theo module (thứ tự khuyến nghị)

## 3.1 Module Auth + User
- [ ] Đăng ký/đăng nhập.
- [ ] Refresh token hoặc phiên đăng nhập cơ bản.
- [ ] Middleware xác thực JWT.
- [ ] API lấy profile người dùng hiện tại.

## 3.2 Module Workspace + Membership
- [ ] Tạo workspace.
- [ ] Mời thành viên.
- [ ] Chấp nhận lời mời.
- [ ] RBAC theo Owner/Member.

## 3.3 Module Wallet + Category (preset/custom)
- [ ] CRUD ví/quỹ.
- [ ] Lock/unlock ví.
- [ ] CRUD danh mục theo cây.
- [ ] Hỗ trợ danh mục preset + custom theo workspace.

## 3.4 Module Transaction
- [ ] Tạo giao dịch Income/Expense.
- [ ] Tạo giao dịch Transfer.
- [ ] Chỉnh sửa/xóa mềm giao dịch.
- [ ] Gắn tag, đính kèm chứng từ.
- [ ] Ghi audit log cho thao tác quan trọng.

## 3.5 Module Debt + Budget (MVP mức cơ bản)
- [ ] Tạo khoản nợ phải thu/phải trả.
- [ ] Ghi nhận thanh toán từng phần.
- [ ] Tạo ngân sách theo danh mục/kỳ.
- [ ] Cảnh báo vượt ngưỡng cơ bản.

## 3.6 Module Dashboard + Report cơ bản
- [ ] API tổng thu/tổng chi/số dư theo filter.
- [ ] API báo cáo thu/chi theo tháng.
- [ ] API dữ liệu biểu đồ cơ bản.

---

## 4) Triển khai FE theo module (song song BE)

## 4.1 Base FE
- [ ] Tạo cấu trúc thư mục chuẩn (`components`, `hooks`, `services`, `types`, `constants`, `schemas`).
- [ ] Tạo API client wrapper (abort + normalize error).
- [ ] Tạo auth guard + route base.

## 4.2 Màn hình chính MVP
- [ ] Đăng nhập/đăng ký.
- [ ] Chọn/tạo workspace.
- [ ] Quản lý ví, danh mục.
- [ ] Form nhập giao dịch nhanh.
- [ ] Dashboard tổng quan + báo cáo tháng.

## 4.3 Chuẩn UX và dữ liệu
- [ ] Mọi lựa chọn phân loại đều hỗ trợ preset + custom.
- [ ] Trạng thái loading/error/empty đầy đủ.
- [ ] Kiểm tra luồng thao tác dưới 10 giây cho nhập giao dịch.

---

## 5) Test checklist

### 5.1 Backend
- [ ] Unit test cho service/use case chính.
- [ ] Integration test cho endpoint core:
  - [ ] Auth
  - [ ] Workspace
  - [ ] Wallet/Category
  - [ ] Transaction
- [ ] Test RBAC không rò dữ liệu giữa workspace.
- [ ] Test migration + seed trên DB rỗng.

### 5.2 Frontend
- [ ] Test flow nhập giao dịch.
- [ ] Test bộ lọc dashboard/report.
- [ ] Test loading/error state.
- [ ] Test quyền hiển thị theo role.

### 5.3 Nghiệp vụ
- [ ] Verify nguyên tắc non-custodial xuyên suốt UI/API/docs.
- [ ] Verify số liệu dashboard khớp dữ liệu giao dịch.
- [ ] Verify quy tắc preset/custom hoạt động đồng nhất.

---

## 6) Security + vận hành

- [ ] Không commit secrets.
- [ ] Bật logging mức phù hợp, không log dữ liệu nhạy cảm.
- [ ] Bật rate-limit cơ bản cho auth endpoints.
- [ ] Cấu hình CORS đúng domain.
- [ ] Chuẩn bị backup/restore cho DB dev/staging.

---

## 7) DoD (Definition of Done) cho mỗi module

- [ ] Code đúng layer theo `CODING_GUIDE.md`.
- [ ] API contract rõ ràng, có ví dụ request/response.
- [ ] Có migration (nếu đổi schema) + không phá dữ liệu cũ.
- [ ] Có test tối thiểu cho luồng chính.
- [ ] Build pass BE + FE.
- [ ] Docs cập nhật (nếu thay đổi nghiệp vụ/contract).

---

## 8) Kế hoạch triển khai 2 tuần (gợi ý)

### Tuần 1
- [ ] Day 1-2: env + DB migration core + auth.
- [ ] Day 3-4: workspace + membership + RBAC.
- [ ] Day 5-6: wallet + category preset/custom.
- [ ] Day 7: transaction create/list/update/delete-soft.

### Tuần 2
- [ ] Day 8-9: debt + budget cơ bản.
- [ ] Day 10: dashboard/report tháng.
- [ ] Day 11: test tích hợp + fix bug.
- [ ] Day 12: hardening bảo mật + logging.
- [ ] Day 13: UAT nội bộ.
- [ ] Day 14: release MVP nội bộ.

---

## 9) Cổng nghiệm thu MVP

- [ ] Người dùng tạo giao dịch <= 10 giây thao tác.
- [ ] Dashboard hiển thị đúng tổng thu/chi/số dư theo bộ lọc.
- [ ] Dữ liệu báo cáo khớp dữ liệu giao dịch.
- [ ] Không rò rỉ dữ liệu giữa các workspace.
- [ ] Toàn bộ secrets lấy từ env, không nằm trong source code.


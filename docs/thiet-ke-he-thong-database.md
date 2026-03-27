# Thiết kế hệ thống database (bám sát nghiệp vụ quản lý dòng tiền)

Tài liệu này mô tả thiết kế CSDL cho hệ thống quản lý chi tiêu/doanh thu/dòng tiền theo hướng ổn định, mở rộng tốt, và bám sát phạm vi nghiệp vụ:

- Hệ thống **không lưu giữ tiền** (non-custodial), chỉ quản lý dữ liệu dòng tiền.
- Hỗ trợ đa workspace (cá nhân/nhóm).
- Hỗ trợ mô hình **preset + custom** cho danh mục và lựa chọn nghiệp vụ.
- Tối ưu cho audit, truy vết, báo cáo, và phân quyền.

Tai lieu migration thuc thi chi tiet: `docs/DATA_MIGRATION_PLAN.md`.

---

## 1) Nguyên tắc thiết kế

- Chuẩn hóa dữ liệu theo 3NF ở phần transactional core.
- Tách rõ:
  - dữ liệu danh mục cấu hình (`Category`, `Tag`, `WalletType`, `Rule`),
  - dữ liệu phát sinh (`Transaction`, `Debt`, `Budget`, `Approval`),
  - dữ liệu truy vết (`AuditLog`, `Notification`).
- Mọi bảng nghiệp vụ chính đều có:
  - `Id` (UUID),
  - `WorkspaceId`,
  - `CreatedAt`, `CreatedBy`,
  - `UpdatedAt`, `UpdatedBy`,
  - soft delete (`IsDeleted`, `DeletedAt`, `DeletedBy`) nếu phù hợp.
- Dùng `DECIMAL(19,4)` cho tiền để tránh sai số.
- Lưu snapshot tiền tệ tại thời điểm giao dịch (không phụ thuộc thay đổi config sau này).

---

## 2) Danh sách bảng cốt lõi

## 2.1 Khối định danh và phân quyền

### `Users`
- **Mục tiêu**: thông tin tài khoản đăng nhập.
- Cột chính:
  - `Id` (PK, UUID)
  - `Email` (UNIQUE)
  - `PasswordHash`
  - `DisplayName`
  - `Status` (Active/Locked)
  - `LastLoginAt`
  - `CreatedAt`, `UpdatedAt`

### `Workspaces`
- **Mục tiêu**: đơn vị quản trị dữ liệu tách biệt.
- Cột chính:
  - `Id` (PK)
  - `Name`
  - `Type` (Personal/Family/Team)
  - `DefaultCurrencyCode` (ISO-4217)
  - `RequireApproval` (bool)
  - `OwnerUserId` (FK -> `Users`)
  - `CreatedAt`, `UpdatedAt`

### `WorkspaceMembers`
- **Mục tiêu**: thành viên và vai trò trong workspace.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK -> `Workspaces`)
  - `UserId` (FK -> `Users`)
  - `Role` (Owner/Member/Viewer)
  - `Status` (Invited/Active/Removed)
  - `JoinedAt`
- Ràng buộc:
  - UNIQUE(`WorkspaceId`, `UserId`)

### `WorkspaceInvitations`
- **Mục tiêu**: quản lý mời thành viên.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `Email`
  - `InviteTokenHash`
  - `Role`
  - `ExpiresAt`
  - `AcceptedAt`

---

## 2.2 Khối ví/quỹ và danh mục preset + custom

### `Wallets`
- **Mục tiêu**: nguồn tiền theo dõi (không phải ví lưu ký).
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `Name`
  - `WalletType` (Cash/Bank/EWallet/Fund/Other)
  - `CurrencyCode`
  - `OpeningBalance` DECIMAL(19,4)
  - `CurrentBalance` DECIMAL(19,4) (denormalized, optional)
  - `IsLocked` (bool)
  - `IsSystem` (bool, cho wallet mặc định)
  - `SortOrder`
- Ràng buộc:
  - UNIQUE(`WorkspaceId`, `Name`, `IsDeleted=false`)

### `Categories`
- **Mục tiêu**: danh mục thu/chi/công nợ/chuyển quỹ; hỗ trợ cây nhiều cấp.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (nullable; null = preset hệ thống toàn cục)
  - `CategoryType` (Income/Expense/Debt/Transfer/Other)
  - `Name`
  - `Code` (slug, optional)
  - `ParentCategoryId` (FK self-reference)
  - `Icon`, `ColorToken`
  - `IsSystem` (preset)
  - `IsActive`
  - `SortOrder`
- Ràng buộc:
  - CHECK không cho vòng lặp cây cha-con.
  - UNIQUE(`WorkspaceId`, `CategoryType`, `Name`, `IsDeleted=false`).

### `Tags`
- **Mục tiêu**: nhãn linh hoạt cho giao dịch.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (nullable cho preset)
  - `Name`
  - `ColorToken`
  - `IsSystem`
  - `IsActive`
- Ràng buộc:
  - UNIQUE(`WorkspaceId`, `Name`, `IsDeleted=false`)

### `TransactionTemplates`
- **Mục tiêu**: mẫu giao dịch nhập nhanh.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `Name`
  - `TransactionType`
  - `DefaultAmount` DECIMAL(19,4) (nullable)
  - `DefaultCategoryId` (FK -> `Categories`, nullable)
  - `DefaultWalletId` (FK -> `Wallets`, nullable)
  - `DefaultNote`
  - `IsSystem`, `IsActive`

---

## 2.3 Khối giao dịch tài chính

### `Transactions`
- **Mục tiêu**: sổ cái phát sinh chính của hệ thống.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `TransactionNo` (mã nghiệp vụ)
  - `TransactionType` (Income/Expense/Transfer/Debt)
  - `Amount` DECIMAL(19,4)
  - `CurrencyCode`
  - `ExchangeRate` DECIMAL(19,8) (nullable)
  - `AmountInWorkspaceCurrency` DECIMAL(19,4)
  - `OccurredAt` (datetime)
  - `RecordedAt` (datetime)
  - `SourceWalletId` (FK -> `Wallets`, nullable)
  - `DestinationWalletId` (FK -> `Wallets`, nullable)
  - `CategoryId` (FK -> `Categories`, nullable)
  - `PayerMemberId` (FK -> `WorkspaceMembers`, nullable)
  - `BeneficiaryMemberId` (FK -> `WorkspaceMembers`, nullable)
  - `Note`
  - `Status` (Draft/PendingApproval/Approved/Rejected/Canceled/Posted)
  - `RecurringTransactionId` (FK, nullable)
  - `ExternalReference` (id đối soát bên ngoài, nullable)
- Ràng buộc:
  - CHECK `Amount > 0`
  - Nếu `TransactionType=Transfer` thì bắt buộc có cả `SourceWalletId` và `DestinationWalletId`, và khác nhau.
  - Nếu `TransactionType=Income|Expense` thì yêu cầu `CategoryId`.
  - UNIQUE(`WorkspaceId`, `TransactionNo`)

### `TransactionTagMappings`
- **Mục tiêu**: many-to-many giữa transaction và tag.
- Cột chính:
  - `TransactionId` (FK -> `Transactions`)
  - `TagId` (FK -> `Tags`)
- Khóa:
  - PK(`TransactionId`, `TagId`)

### `TransactionAttachments`
- **Mục tiêu**: hóa đơn/chứng từ.
- Cột chính:
  - `Id` (PK)
  - `TransactionId` (FK -> `Transactions`)
  - `FileName`
  - `FileUrl`
  - `MimeType`
  - `FileSize`
  - `UploadedBy`
  - `UploadedAt`

---

## 2.4 Khối giao dịch lặp, công nợ, ngân sách

### `RecurringTransactions`
- **Mục tiêu**: định nghĩa lịch phát sinh tự động.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `Name`
  - `TransactionType`
  - `Amount` DECIMAL(19,4)
  - `CategoryId` (FK)
  - `SourceWalletId` (FK, nullable)
  - `DestinationWalletId` (FK, nullable)
  - `RecurrencePattern` (Daily/Weekly/Monthly/Quarterly/Yearly/Custom)
  - `CronExpression` (nullable)
  - `StartDate`
  - `EndDate` (nullable)
  - `NextRunAt`
  - `IsActive`

### `Debts`
- **Mục tiêu**: khoản phải thu/phải trả.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `DebtType` (Receivable/Payable)
  - `CounterpartyName`
  - `CounterpartyContact` (nullable)
  - `PrincipalAmount` DECIMAL(19,4)
  - `OutstandingAmount` DECIMAL(19,4)
  - `CurrencyCode`
  - `DueDate` (nullable)
  - `Status` (NotDue/Due/Overdue/Completed)
  - `Note`

### `DebtPayments`
- **Mục tiêu**: lịch sử thanh toán nợ.
- Cột chính:
  - `Id` (PK)
  - `DebtId` (FK -> `Debts`)
  - `PaymentAmount` DECIMAL(19,4)
  - `PaymentDate`
  - `WalletId` (FK -> `Wallets`, nullable)
  - `TransactionId` (FK -> `Transactions`, nullable)
  - `Note`
- Ràng buộc:
  - CHECK `PaymentAmount > 0`

### `Budgets`
- **Mục tiêu**: cấu hình ngân sách theo kỳ.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `BudgetScopeType` (Category/Wallet/Project/Member/Global)
  - `CategoryId` (nullable)
  - `WalletId` (nullable)
  - `MemberId` (nullable)
  - `PeriodType` (Monthly/Quarterly/Yearly/Custom)
  - `PeriodStart`, `PeriodEnd`
  - `PlannedAmount` DECIMAL(19,4)
  - `WarningThreshold80` (bool)
  - `WarningThreshold100` (bool)
  - `IsActive`

### `BudgetSnapshots`
- **Mục tiêu**: snapshot nhanh cho báo cáo ngân sách.
- Cột chính:
  - `Id` (PK)
  - `BudgetId` (FK -> `Budgets`)
  - `SnapshotAt`
  - `ActualAmount` DECIMAL(19,4)
  - `VarianceAmount` DECIMAL(19,4)
  - `UtilizationPercent` DECIMAL(7,4)

---

## 2.5 Khối duyệt chi, thông báo, audit, lock kỳ

### `ApprovalPolicies`
- **Mục tiêu**: cấu hình ngưỡng duyệt theo workspace.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `MinAmountForApproval` DECIMAL(19,4)
  - `ApprovalLevels` (hiện tại hỗ trợ 1, mở rộng sau)
  - `IsActive`

### `TransactionApprovals`
- **Mục tiêu**: log từng lần duyệt/từ chối.
- Cột chính:
  - `Id` (PK)
  - `TransactionId` (FK -> `Transactions`)
  - `ApproverMemberId` (FK -> `WorkspaceMembers`)
  - `Decision` (Approved/Rejected)
  - `Comment`
  - `DecidedAt`

### `Notifications`
- **Mục tiêu**: thông báo in-app/email.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK, nullable)
  - `UserId` (FK -> `Users`)
  - `Channel` (InApp/Email)
  - `Type` (DebtDue/BudgetExceeded/ApprovalNeeded/RecurringUpcoming/Anomaly)
  - `Title`
  - `Content`
  - `IsRead`
  - `SentAt`
  - `ReadAt` (nullable)
  - `ReferenceType`, `ReferenceId` (nullable)

### `AuditLogs`
- **Mục tiêu**: truy vết thay đổi dữ liệu quan trọng.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK, nullable)
  - `ActorUserId` (FK -> `Users`)
  - `EntityName`
  - `EntityId`
  - `Action` (Create/Update/Delete/Approve/Reject/Login/Export)
  - `BeforeData` (JSON, nullable)
  - `AfterData` (JSON, nullable)
  - `IpAddress`
  - `UserAgent`
  - `CreatedAt`

### `PeriodLocks`
- **Mục tiêu**: khóa dữ liệu theo kỳ kế toán quản trị.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `PeriodType` (Monthly/Quarterly)
  - `PeriodStart`, `PeriodEnd`
  - `IsLocked`
  - `LockedBy`, `LockedAt`
  - `UnlockedBy` (nullable), `UnlockedAt` (nullable)
  - `Reason` (nullable)
- Ràng buộc:
  - UNIQUE(`WorkspaceId`, `PeriodType`, `PeriodStart`, `PeriodEnd`)

---

## 2.6 Khối cá nhân hóa và rule engine

### `UserFinancialProfiles`
- **Mục tiêu**: hồ sơ tài chính cá nhân.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `UserId` (FK)
  - `ManagementStyle` (Strict/Balanced/Flexible)
  - `Priorities` (JSON: saving/investment/debt/emergency)
  - `IncomeCycleType` (Monthly/Project/Seasonal)
  - `SettingsJson`
- Ràng buộc:
  - UNIQUE(`WorkspaceId`, `UserId`)

### `DashboardLayouts`
- **Mục tiêu**: lưu layout dashboard theo ngữ cảnh.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `UserId` (FK)
  - `Name`
  - `ContextType` (Personal/Family/Work)
  - `WidgetsConfigJson`
  - `IsDefault`

### `AlertRules`
- **Mục tiêu**: quy tắc cảnh báo tùy biến.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `CreatedBy` (FK -> `Users`)
  - `RuleType` (BudgetThreshold/Anomaly/DebtDue/CashShortage/Custom)
  - `ConditionJson`
  - `ChannelsJson` (InApp, Email...)
  - `Frequency` (Realtime/Daily/Weekly)
  - `IsActive`

### `AutoCategorizationRules`
- **Mục tiêu**: tự gợi ý/auto-tag.
- Cột chính:
  - `Id` (PK)
  - `WorkspaceId` (FK)
  - `RuleName`
  - `MatchType` (Keyword/Vendor/AmountRange/Pattern)
  - `PatternValue`
  - `ApplyCategoryId` (FK -> `Categories`, nullable)
  - `ApplyTagId` (FK -> `Tags`, nullable)
  - `Priority`
  - `IsActive`

---

## 3) Quan hệ chính (tóm tắt)

- `Users` 1-n `WorkspaceMembers`
- `Workspaces` 1-n `WorkspaceMembers`, `Wallets`, `Categories`, `Transactions`, `Budgets`, `Debts`
- `Transactions` n-1 `Categories`, n-1 `Wallets`
- `Transactions` n-n `Tags` qua `TransactionTagMappings`
- `Debts` 1-n `DebtPayments`
- `Transactions` 1-n `TransactionApprovals`
- `Budgets` 1-n `BudgetSnapshots`

---

## 4) Chỉ mục (index) đề xuất

## 4.1 Index bắt buộc

- `Transactions`:
  - INDEX(`WorkspaceId`, `OccurredAt`)
  - INDEX(`WorkspaceId`, `TransactionType`, `OccurredAt`)
  - INDEX(`WorkspaceId`, `CategoryId`, `OccurredAt`)
  - INDEX(`WorkspaceId`, `SourceWalletId`, `OccurredAt`)
  - INDEX(`WorkspaceId`, `Status`, `OccurredAt`)
- `Debts`:
  - INDEX(`WorkspaceId`, `Status`, `DueDate`)
- `Budgets`:
  - INDEX(`WorkspaceId`, `PeriodStart`, `PeriodEnd`, `IsActive`)
- `AuditLogs`:
  - INDEX(`WorkspaceId`, `EntityName`, `EntityId`, `CreatedAt`)

## 4.2 Unique index quan trọng

- `WorkspaceMembers` UNIQUE(`WorkspaceId`, `UserId`)
- `Wallets` UNIQUE(`WorkspaceId`, `Name`, `IsDeleted=false`)
- `Categories` UNIQUE(`WorkspaceId`, `CategoryType`, `Name`, `IsDeleted=false`)
- `Transactions` UNIQUE(`WorkspaceId`, `TransactionNo`)

---

## 5) Trạng thái nghiệp vụ chuẩn hóa

- `TransactionStatus`: `Draft`, `PendingApproval`, `Approved`, `Rejected`, `Canceled`, `Posted`
- `DebtStatus`: `NotDue`, `Due`, `Overdue`, `Completed`
- `InvitationStatus`: `Invited`, `Active`, `Removed`
- `MemberRole`: `Owner`, `Member`, `Viewer`

---

## 6) Quy tắc dữ liệu quan trọng

- Không cho xóa cứng dữ liệu giao dịch đã phát sinh; ưu tiên soft delete + audit.
- Nếu kỳ đã lock (`PeriodLocks.IsLocked = true`) thì cấm create/update/delete transaction trong kỳ đó (trừ Owner mở khóa).
- `CurrentBalance` của ví nên cập nhật theo event/outbox hoặc service transaction-safe; không tính ở UI.
- Với `Transfer`, hệ thống ghi nhận đối ứng nguồn/đích nhất quán trong cùng transaction DB.
- Danh mục preset (`IsSystem=true`, `WorkspaceId=null`) không cho sửa cứng; chỉ cho clone/custom theo workspace nếu cần.

---

## 7) Partitioning và lưu trữ dài hạn (khi scale)

- Cân nhắc partition bảng `Transactions` theo `WorkspaceId` hoặc theo tháng (`OccurredAt`) khi dữ liệu lớn.
- `AuditLogs` và `Notifications` có thể áp dụng chính sách archive theo tuổi dữ liệu.
- Dùng read model/materialized view cho dashboard truy vấn nặng.

---

## 8) Mapping sang layer code (.NET)

- `Domain/Entity`:
  - Mỗi bảng chính là 1 entity tương ứng.
- `Infrastructure/Persistence/Configurations`:
  - Mỗi bảng có 1 file cấu hình `{Entity}Configuration.cs`.
- `Application/DTOs`:
  - Tách DTO cho create/update/get/list theo từng aggregate.
- `Repository`:
  - Query tổng hợp báo cáo đặt ở repository chuyên biệt để tối ưu SQL/index.

---

## 9) Danh sách migration gợi ý theo pha

- **Pha 1 (MVP core)**:
  - `Users`, `Workspaces`, `WorkspaceMembers`, `Wallets`, `Categories`, `Transactions`, `Debts`, `DebtPayments`, `Budgets`, `Notifications`.
- **Pha 2 (control + quality)**:
  - `ApprovalPolicies`, `TransactionApprovals`, `AuditLogs`, `TransactionTemplates`, `PeriodLocks`, `TransactionAttachments`.
- **Pha 3 (advanced personalization + scaling)**:
  - `UserFinancialProfiles`, `DashboardLayouts`, `AlertRules`, `AutoCategorizationRules`, `BudgetSnapshots`.

---

## 10) Kết luận

Thiết kế trên ưu tiên:
- đúng phạm vi quản lý dòng tiền (không custody),
- chắc về integrity và audit,
- mở rộng được cho báo cáo, cá nhân hóa, tự động hóa,
- phù hợp triển khai theo Clean Architecture trên .NET.

Khi triển khai thực tế, có thể điều chỉnh chi tiết cột/enum theo chuẩn coding guide, nhưng cần giữ nguyên các nguyên tắc dữ liệu lõi và ràng buộc nghiệp vụ trong tài liệu này.

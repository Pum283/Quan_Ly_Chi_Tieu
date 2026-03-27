# Data Design & Migration Plan (Chot)

Tai lieu nay chot phan **2) Thiet ke du lieu va migration plan** de team co the trien khai code ngay theo mot huong duy nhat.

Can cu:
- `docs/thiet-ke-he-thong-database.md`
- `docs/yeu-cau-chuc-nang-web-quan-ly-chi-tieu.md`
- Nguyen tac non-custodial + preset/custom + role `Admin/Owner/Member/Viewer`

---

## 1) Chot enum va trang thai nghiep vu

## 1.1 Role va membership
- `SystemRole`: `Admin`, `User`
- `WorkspaceRole`: `Owner`, `Member`, `Viewer`
- `MembershipStatus`: `Invited`, `Active`, `Removed`

## 1.2 Transaction
- `TransactionType`: `Income`, `Expense`, `Transfer`, `Debt`
- `TransactionStatus`: `Draft`, `PendingApproval`, `Approved`, `Rejected`, `Canceled`, `Posted`

## 1.3 Debt
- `DebtType`: `Receivable`, `Payable`
- `DebtStatus`: `NotDue`, `Due`, `Overdue`, `Completed`

## 1.4 Budget va canh bao
- `BudgetScopeType`: `Category`, `Wallet`, `Project`, `Member`, `Global`
- `PeriodType`: `Monthly`, `Quarterly`, `Yearly`, `Custom`

## 1.5 Notification
- `NotificationChannel`: `InApp`, `Email`
- `NotificationType`: `DebtDue`, `BudgetExceeded`, `ApprovalNeeded`, `RecurringUpcoming`, `Anomaly`

---

## 2) Migration theo pha (bat buoc)

## Pha 1 - Core (MVP)

### 2.1 Bang can tao
- `Users`
- `Workspaces`
- `WorkspaceMembers`
- `WorkspaceInvitations`
- `Wallets`
- `Categories`
- `Tags`
- `Transactions`
- `TransactionTagMappings`
- `Debts`
- `DebtPayments`
- `Budgets`
- `Notifications`

### 2.2 Rang buoc bat buoc
- UNIQUE(`WorkspaceId`, `UserId`) tren `WorkspaceMembers`
- UNIQUE(`WorkspaceId`, `TransactionNo`) tren `Transactions`
- UNIQUE (`WorkspaceId`, `Name`, soft-delete-filter) tren `Wallets`
- UNIQUE (`WorkspaceId`, `CategoryType`, `Name`, soft-delete-filter) tren `Categories`
- CHECK `Amount > 0` tren `Transactions`, `DebtPayments`
- CHECK logic transfer: `SourceWalletId` va `DestinationWalletId` phai co va khac nhau

### 2.3 Index bat buoc
- `Transactions(WorkspaceId, OccurredAt)`
- `Transactions(WorkspaceId, TransactionType, OccurredAt)`
- `Transactions(WorkspaceId, CategoryId, OccurredAt)`
- `Transactions(WorkspaceId, SourceWalletId, OccurredAt)`
- `Transactions(WorkspaceId, Status, OccurredAt)`
- `Debts(WorkspaceId, Status, DueDate)`
- `Budgets(WorkspaceId, PeriodStart, PeriodEnd, IsActive)`

---

## Pha 2 - Kiem soat va truy vet

### 2.4 Bang can tao
- `ApprovalPolicies`
- `TransactionApprovals`
- `AuditLogs`
- `TransactionAttachments`
- `TransactionTemplates`
- `PeriodLocks`
- `RecurringTransactions`

### 2.5 Rang buoc nghiep vu
- UNIQUE(`WorkspaceId`, `PeriodType`, `PeriodStart`, `PeriodEnd`) tren `PeriodLocks`
- Neu ky bi lock thi cam create/update/delete transaction trong ky
- Log audit cho cac action quan trong: create/update/delete/approve/reject/export

---

## Pha 3 - Ca nhan hoa va automation

### 2.6 Bang can tao
- `UserFinancialProfiles`
- `DashboardLayouts`
- `AlertRules`
- `AutoCategorizationRules`
- `BudgetSnapshots`

---

## 3) Seed data plan (bat buoc)

## 3.1 Seed he thong
- Category preset cho `Income` va `Expense`
- Tag preset co ban
- WalletType preset

## 3.2 Seed workspace mau (dev)
- 1 user owner
- 1 workspace personal
- 2 wallet mau (`Tien mat`, `Ngan hang`)
- 1 so category custom mau

## 3.3 Nguyen tac seed
- Seed idempotent (chay lai khong tao trung)
- Khong seed secret that
- Tach seed he thong va seed dev

---

## 4) Thu tu trien khai migration trong code

1. Tao Domain entities + enums.
2. Tao EF configurations trong `Infrastructure/Persistence/Configurations`.
3. Cap nhat `AppDbContext` voi DbSet theo pha.
4. Tao migration:
   - `dotnet ef migrations add InitialCore --project Infrastructure --startup-project API`
   - `dotnet ef database update --project Infrastructure --startup-project API`
5. Tao migration pha 2/3 khi core da on dinh.
6. Viet seed runner va goi trong startup o moi truong dev.

---

## 5) Quy tac review migration

- Khong sua migration da ap dung production.
- Moi thay doi schema phai qua migration moi.
- Moi migration phai co:
  - mo ta ro muc dich
  - rollback strategy
  - danh gia tac dong du lieu
- Bat buoc review index/constraint truoc merge.

---

## 6) Tieu chi hoan tat section 2

Section 2 duoc coi la hoan tat khi:
- Enum/trang thai nghiep vu da chot (muc 1).
- Ke hoach migration 3 pha da chot (muc 2).
- Seed plan da chot (muc 3).
- Thu tu trien khai va command da chot (muc 4).
- Quy tac review migration da chot (muc 5).

Trang thai hien tai: **CHOT**.

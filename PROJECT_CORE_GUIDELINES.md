# PROJECT CORE GUIDELINES (BE)

Tài liệu này là bản rút gọn cốt lõi, bám sát `docs/CODING_GUIDE.md`, dùng để onboarding nhanh cho AI agent và developer mới.

## 1) Kiến trúc bắt buộc

- Áp dụng Clean Architecture với 4 layer: `API` -> `Infrastructure` -> `Application` -> `Domain`.
- `Domain` là lõi nghiệp vụ, không phụ thuộc bất kỳ layer nào.
- `Application` chỉ phụ thuộc `Domain`.
- `Infrastructure` phụ thuộc `Application` và `Domain`.
- `API` có thể phụ thuộc tất cả layer còn lại.

## 2) Dependency rules (không được vi phạm)

- `Domain`:
  - Khong import từ `Application`, `Infrastructure`, `API`.
- `Application`:
  - Khong import từ `Infrastructure`, `API`.
  - Chỉ dùng interface và model của chính nó + `Domain`.
- `Infrastructure`:
  - Khong import từ `API`.
- `API`:
  - Được gọi `Application` services/DTOs, không chứa business logic.

## 3) Quy ước đặt tên

- `PascalCase`:
  - Class, interface member public, DTO, enum, namespace.
- Interface:
  - Bắt đầu bằng `I`, ví dụ `IUserRepository`, `IUserService`.
- `camelCase`:
  - Parameter, local variable.
- Private field:
  - `camelCase` + prefix `_`, ví dụ `_userRepository`.
- Async method:
  - Kết thúc bằng `Async`, trả `Task` hoặc `Task<T>`.
- Mỗi file chỉ chứa 1 class/enum chính, tên file trùng tên class/enum.

## 4) Cấu trúc thư mục chuẩn theo dự án

```text
Quan_Ly_Chi_Tieu_BE/
  API/
    Controllers/
    Contracts/            # optional khi khác DTO Application
    Middlewares/
    Attributes/
  Application/
    DTOs/
      <Entity>/
    Interfaces/
      Repositories/
      Services/
    Service/              # hoặc Features/ nếu dùng MediatR
    Common/
  Domain/
    Entity/
    Enum/
    Common/
  Infrastructure/
    Repositories/
    Services/
    Persistence/
      Configurations/
      Migrations/
      Interceptors/
```

## 5) Vị trí file và trách nhiệm hàm

- `Domain/Entity`:
  - Định nghĩa entity, business state cốt lõi.
  - Ưu tiên kế thừa `BaseEntity` hoặc `BaseAuditableEntity`.
- `Domain/Enum`:
  - Enum theo nghiệp vụ.
- `Application/DTOs/<Entity>`:
  - `Create{Entity}Dto`, `Update{Entity}Dto`, `{Entity}Dto`.
- `Application/Interfaces/Repositories`:
  - Contract truy cập dữ liệu.
- `Application/Interfaces/Services`:
  - Contract business use case.
- `Application/Service` (hoặc `Features`):
  - Chứa business logic, validation rule nghiệp vụ, orchestration.
- `Infrastructure/Repositories`:
  - Chỉ data access logic (EF Core, query, include, paging).
- `Infrastructure/Persistence/Configurations`:
  - Mapping entity bằng `IEntityTypeConfiguration<T>`.
- `API/Controllers`:
  - Thin controller: nhận request -> gọi service -> trả response.
  - Không viết query DB và business rule trực tiếp.

## 6) Chuẩn code bắt buộc

- Dùng async/await cho mọi I/O với DB/network.
- Luôn truyền `CancellationToken` cho async methods.
- Null safety:
  - Dùng nullable reference types đúng nghĩa.
  - Khai báo default value cho non-nullable khi phù hợp.
- Truy vấn EF:
  - Ưu tiên `ToListAsync`, `FirstOrDefaultAsync`, `AnyAsync`.
  - Tránh sync query gây blocking.
- Error handling:
  - Ưu tiên `OperationResult` cho flow nghiệp vụ.
  - Dùng `NotFoundException`, `BusinessException` cho lỗi domain phù hợp.

## 7) API và DI

- Controller route theo chuẩn `api/[controller]`.
- Dùng middleware xử lý exception toàn cục.
- Constructor injection bắt buộc, không dùng service locator/property injection.
- Đăng ký dependency tại `Infrastructure/DependencyInjection.cs`.

## 8) Quy tắc cấu hình và secrets

- Không lưu key/secret/thông tin nhạy cảm trong `appsettings.json`.
- Bắt buộc dùng environment variables cho:
  - Connection strings.
  - JWT secrets/signing keys.
  - API keys tích hợp bên thứ 3.
  - SMTP credentials và các thông tin đăng nhập dịch vụ.
- `appsettings.json` chỉ giữ cấu hình không nhạy cảm (feature flags chung, timeout mặc định, logging levels).
- Tên biến môi trường dùng `UPPER_SNAKE_CASE`, rõ ngữ nghĩa, ví dụ:
  - `DB_CONNECTION_STRING`
  - `JWT_SIGNING_KEY`
  - `SMTP_USERNAME`
  - `SMTP_PASSWORD`
- Không commit file chứa secret thật vào source control.
- Mọi môi trường (`dev/staging/prod`) phải có bộ env riêng.

## 9) Quy tắc EF Core

- Mapping tách riêng từng file configuration (`{Entity}Configuration.cs`).
- Tránh nhồi quá nhiều config trong `OnModelCreating`.
- Truy vấn lớn phải có pagination.
- Tạo migration bằng lệnh chuẩn, không sửa migration cũ đã chạy production.

## 10) Checklist thêm mới feature

- [ ] Tạo entity trong `Domain/Entity/`.
- [ ] Tạo enum/domain common (nếu cần).
- [ ] Tạo configuration trong `Infrastructure/Persistence/Configurations/`.
- [ ] Tạo DTOs trong `Application/DTOs/{Entity}/`.
- [ ] Tạo repository interface trong `Application/Interfaces/Repositories/`.
- [ ] Implement repository trong `Infrastructure/Repositories/`.
- [ ] Tạo service interface trong `Application/Interfaces/Services/`.
- [ ] Implement service trong `Application/Service/` (hoặc `Features/` nếu dùng MediatR).
- [ ] Tạo controller trong `API/Controllers/`.
- [ ] Register DI trong `Infrastructure/DependencyInjection.cs`.
- [ ] Tạo migration và verify endpoint qua Swagger.

## 11) Code review checklist

- [ ] Đúng naming conventions.
- [ ] Đúng vị trí file theo layer.
- [ ] Không vi phạm dependency rules.
- [ ] Controller thin, không business logic.
- [ ] Service có validation + business rule.
- [ ] Repository chỉ làm data access.
- [ ] Có async/await + `CancellationToken`.
- [ ] Xử lý null an toàn.
- [ ] Error handling theo `OperationResult`/domain exception.
- [ ] Không hardcode secrets và không để secrets trong `appsettings.json`.

## 12) Tài liệu bắt buộc đọc trước khi code

- `docs/CODING_GUIDE.md` (nguồn chuẩn chi tiết).
- `README.md` (tổng quan project và cách chạy).

## 13) Nguyên tắc khi AI/Coder triển khai

- Nếu có xung đột giữa tài liệu này và code hiện tại, ưu tiên:
  1. `docs/CODING_GUIDE.md`
  2. Kiến trúc layer thực tế trong solution
  3. Tài liệu này
- Không code tắt phá layer để "chạy nhanh".
- Mọi thay đổi lớn về kiến trúc phải cập nhật lại `docs/CODING_GUIDE.md` và file này.

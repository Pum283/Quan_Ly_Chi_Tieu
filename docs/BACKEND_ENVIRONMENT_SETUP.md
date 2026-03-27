# Backend Environment Setup

Tài liệu này mô tả cách cấu hình môi trường cho Backend theo nguyên tắc:
- Không lưu secrets trong `appsettings.json`.
- Secrets lấy từ environment variables.

## 1) Các biến môi trường bắt buộc

- `DB_CONNECTION_STRING`
- `JWT_SIGNING_KEY`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `JWT_EXPIRES_MINUTES`
- `CORS_ALLOWED_ORIGINS`
- `GOOGLE_AUTH_CLIENT_ID`
- `GOOGLE_AUTH_CLIENT_SECRET`
- `GOOGLE_AUTH_REDIRECT_URI`

Biến tùy chọn (nếu dùng email):
- `SMTP_HOST`
- `SMTP_PORT`
- `SMTP_USERNAME`
- `SMTP_PASSWORD`
- `SMTP_ENABLE_SSL`
- `SMTP_DISPLAY_NAME`

## 2) File môi trường sử dụng

## 3) Quy tắc ưu tiên cấu hình

1. Environment variables (ưu tiên cao nhất).
2. `appsettings.json` (chỉ để giá trị không nhạy cảm và fallback không-secret).

Lưu ý: hiện tại connection string được đọc theo thứ tự:
- `DB_CONNECTION_STRING` -> nếu không có thì fallback `ConnectionStrings:DefaultConnection`.

## 4) Cách chạy local

1. Sử dụng trực tiếp `API/.env` cho local development.
2. Điền giá trị thật.
3. Nạp biến môi trường vào shell/IDE trước khi chạy API.
4. Chạy API:
   - `dotnet run --project API`

## 5) Bảo mật

- Không commit file env chứa giá trị thật.
- Rotate định kỳ `JWT_SIGNING_KEY` và credentials SMTP.
- Mỗi môi trường dev/staging/prod phải dùng bộ env tách biệt.

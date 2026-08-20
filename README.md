# Ecommerce
### Ứng dụng quản lý sinh viên và theo dõi đóng góp của sinh viên trong nhóm với giao diện Kanban trực quan

## Tính năng
- Đăng nhập, đăng ký thông thường và đăng ký bằng OAuth2
- Quản lý thông tin người dùng
- Tạo, xóa, sửa sản phẩm
- Đặt hàng, thêm vào giỏ hàng, thêm vào yêu thích
- Bình luận về sản phẩm


## Công nghệ sử dụng
- **Backend:** ASP.NET Core 8, SQL Server
- **Containerization:** Docker, Docker Compose

## Yêu cầu hệ thống
- Docker & Docker Compose
- .NET SDK 8.0
- Visual Studio 2022
- SQL Server

## Cài đặt & Chạy dự án

### Sử dụng Docker (khuyến nghị)

```bash
git clone https://github.com/nnkongh/Project-Ecommerce-Clean-Architecture
cd FINAL_PROJECT
docker compose up --build
```

### Chạy thủ công

**Backend (ASP.NET Core)**

```bash
dotnet restore
dotnet ef database update 
dotnet run
```

## Kiến trúc hệ thống

<img width="700" alt="architecture" src="https://github.com/user-attachments/assets/ed9fdea9-bf69-4bb1-8a2c-892fe3e5ef5d" />

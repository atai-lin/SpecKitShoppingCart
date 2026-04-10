# Quickstart: 商品瀏覽功能開發指南

## 1. 啟動準備
- **開發環境**: .NET 10 SDK, Visual Studio 2025 (或 VS Code)。
- **資料庫**: 啟動 SQL Server (LocalDB 或 Docker 容器)。

## 2. 初始設定
1. **Migrations**: 執行資料庫更新。
   ```bash
   dotnet ef database update
   ```
2. **Seed Data**: 專案啟動時會自動寫入測試用的 50 筆商品資料 (10k 壓力測試需手動執行腳本)。

## 3. 核心實作路徑
- **Model**: `src/Models/Product.cs`
- **Service**: `src/Services/ProductService.cs` (實作搜尋、分頁、排序邏輯)
- **UI**: `src/Pages/Products/Index.cshtml` (使用 Tailwind CSS 渲染卡片視圖)

## 4. 關鍵測試情境 (P1)
- **搜尋測試**: 輸入「電競」應回傳符合名稱或描述的商品。
- **分頁測試**: 點擊第 2 頁應顯示第 21-40 筆商品。
- **排序測試**: 按價格「由低到高」排列，確認首位商品最便宜。

## 5. 常用指令
```bash
# 執行專案
dotnet run --project src

# 執行測試
dotnet test tests/UnitTests
```

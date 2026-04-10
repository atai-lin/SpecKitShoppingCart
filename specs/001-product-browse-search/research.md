# Research: 商品瀏覽與搜尋實作分析

## 核心問題研究

### 1. 關鍵字完全比對 (Exact Keyword Match)
- **需求分析**: 用戶要求「關鍵字完全比對」。在商品搜尋情境中，這通常指搜尋字串需與商品名稱 (Name) 或特定屬性完全一致。
- **實作選擇**:
    - **精確匹配**: 使用 `p.Name == keyword`。EF Core 會轉譯為 SQL 的 `WHERE Name = '...'`。
    - **全字比對 (Full-text Search)**: 若需在描述中尋找「精確單字」，則需使用 `EF.Functions.FreeText` 或 `Contains` 配合全文檢索索引。
- **最終決定**: 實作精確名稱匹配 (`==`)。若名稱不匹配，再退而求其次檢查是否包含該字串 (`Contains`)，但以精確匹配為優先權最高。
- **Rationale**: 確保用戶輸入完整名稱時能精準定位商品，符合「完全比對」字面意義。

### 2. Razor Pages 分頁最佳實踐
- **實作選擇**: 採用 **PaginatedList Pattern**。
- **細節**: 
    - 建立泛型 `PaginatedList<T>` 類別，封裝 `PageIndex`, `TotalPages`, `HasPreviousPage`, `HasNextPage` 等元數據。
    - 資料庫端使用 `.Skip((pageIndex - 1) * pageSize).Take(pageSize)`。
    - 每頁筆數固定為 **20** 筆 (依用戶要求)。
- **效能優化**: 查詢時使用 `.AsNoTracking()` 以減少記憶體開銷。

### 3. 動態排序實作 (價格、上架時間)
- **實作選擇**: 使用 `Switch` 語句配合 LINQ 或 **Expression Tree** 擴充方法。
- **細節**: 
    - 支援 `Price` (decimal) 與 `UploadTime` (DateTime) 排序。
    - 預設排序為 `UploadTime` 降冪 (最新上架)。
- **Rationale**: `Switch` 語句對於少量固定欄位 (Price, UploadTime) 較為直觀且型別安全；若欄位增多則考慮 Expression Tree。

## 技術決策總結

| 項目 | 決策 | 理由 |
| --- | --- | --- |
| 搜尋邏輯 | `p.Name == keyword` 優先 | 符合用戶「完全比對」要求 |
| 分頁方式 | Offset-based Paging (20/page) | 標準實作，易於 SEO 與 URL 分享 |
| 排序實作 | LINQ `OrderBy` / `OrderByDescending` | 高效且受 EF Core 原生支援 |
| 前端框架 | Razor Pages + Tailwind CSS | 符合專案憲法與規格要求 |

## 待澄清事項 (NEEDS CLARIFICATION)
- **上架時間欄位名稱**: 實體模型中預計命名為 `CreatedTime` 或 `UploadTime`？(暫定 `CreatedTime`)
- **多分類篩選邏輯**: 規格書 FR-003 提到 OR 邏輯，需確認 UI 是否支援多選。 (暫定 URL 傳遞陣列)

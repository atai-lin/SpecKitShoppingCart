# Tasks: 商品瀏覽與搜尋 (Product Browse & Search)

**Input**: Design documents from `/specs/001-product-browse-search/`
**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, contracts/

## Format: `- [ ] [TaskID] [P?] [Story?] Description with file path`

- **[P]**: 可並行執行 (不同檔案，無未完成依賴)
- **[US1..4]**: 對應使用者情境 (User Story)
- 檔案路徑基於 `src/` 與 `tests/` 根目錄

---

## Phase 1: Setup (專案初始化)

**目的**: 建立基礎專案結構與配置

- [ ] T001 建立 Razor Pages 專案結構與目錄 `src/`, `tests/`
- [ ] T002 安裝核心依賴 (EF Core, SQL Server, Tailwind CSS) 在 `src/ShoppingCart.csproj`
- [ ] T003 [P] 配置 Tailwind CSS 與共用樣式在 `src/wwwroot/css/site.css`
- [ ] T004 [P] 設定 `src/appsettings.json` 資料庫連線字串與開發環境

---

## Phase 2: Foundational (核心基礎設施)

**目的**: 建立所有使用者情境共用的資料模型、資料庫上下文與工具類別

**⚠️ CRITICAL**: 此階段必須完成後才能開始任何使用者情境的開發

- [ ] T005 [P] 建立實體模型 `Category` 在 `src/Models/Category.cs`
- [ ] T006 [P] 建立實體模型 `Product` 在 `src/Models/Product.cs`
- [ ] T007 建立 `AppDbContext` 並設定 Fluent API 關係與索引在 `src/Data/AppDbContext.cs`
- [ ] T008 執行初始 Migration 並更新資料庫 `src/Data/Migrations/`
- [ ] T009 [P] 實作泛型分頁工具類別 `PaginatedList<T>` 在 `src/Models/PaginatedList.cs`
- [ ] T010 [P] 建立基礎 Service 介面 `IProductService` 在 `src/Services/IProductService.cs`
- [ ] T011 [P] 實作共用 Layout 導覽列與頁尾在 `src/Pages/Shared/_Layout.cshtml`

**Checkpoint**: 基礎設施就緒 - 實體模型與分頁邏輯已可供使用

---

## Phase 3: User Story 1 - 分頁瀏覽商品 (Priority: P1) 🎯 MVP

**目標**: 使用者能以每頁 20 筆的方式瀏覽商品列表。

**獨立測試**: 訪問 `/Products`，驗證顯示 20 筆商品且分頁控制項可正常切換。

- [ ] T012 [P] [US1] 建立 `ProductDto` 在 `src/Models/Dtos/ProductDto.cs`
- [ ] T013 [US1] 實作 `ProductService.GetProductsAsync` (支援分頁) 在 `src/Services/ProductService.cs`
- [ ] T014 [US1] 建立商品列表頁面模型 `IndexModel` 在 `src/Pages/Products/Index.cshtml.cs`
- [ ] T015 [US1] 建立商品列表視圖並套用 Tailwind 網格佈局在 `src/Pages/Products/Index.cshtml`
- [ ] T016 [US1] 實作分頁導覽組件 (Next/Prev) 在 `src/Pages/Products/Index.cshtml`
- [ ] T017 [US1] 實作商品種子資料腳本 (Seed Data) 在 `src/Data/DbInitializer.cs`

**Checkpoint**: US1 完成後，系統已具備基礎商品展示功能 (MVP)。

---

## Phase 4: User Story 2 - 關鍵字搜尋 (Priority: P1)

**目標**: 使用者可透過關鍵字精確或模糊搜尋商品名稱。

**獨立測試**: 搜尋完整商品名稱應排在首位，搜尋部分字串應顯示相關結果。

- [ ] T018 [US2] 在 `ProductService` 實作搜尋過濾邏輯 (優先匹配全名) 在 `src/Services/ProductService.cs`
- [ ] T019 [US2] 在 `Index.cshtml` 增加搜尋列 UI 並綁定 `q` 參數
- [ ] T020 [US2] 實作搜尋結果回顯與「找不到商品」提示在 `src/Pages/Products/Index.cshtml`

---

## Phase 5: User Story 3 - 分類篩選 (Priority: P2)

**目標**: 使用者可按一個或多個分類過濾商品。

**獨立測試**: 選擇特定分類後，列表應僅顯示該分類商品，且分頁狀態應保持。

- [ ] T021 [US3] 在 `ProductService` 實作多分類過濾邏輯 (OR 邏輯) 在 `src/Services/ProductService.cs`
- [ ] T022 [US3] 實作側欄分類選單 UI 在 `src/Pages/Products/Index.cshtml`
- [ ] T023 [US3] 處理 URL 查詢參數中的多分類 ID 綁定在 `src/Pages/Products/Index.cshtml.cs`

---

## Phase 6: User Story 4 - 按價格與熱銷程度排序 (Priority: P2)

**目標**: 支援按價格 (高/低) 與銷售量 (熱銷) 排序。

**獨立測試**: 切換排序選項，驗證商品順序是否符合預期。

- [ ] T024 [US4] 在 `ProductService` 實作動態排序邏輯 (Switch 語句) 在 `src/Services/ProductService.cs`
- [ ] T025 [US4] 在 UI 增加排序下拉選單在 `src/Pages/Products/Index.cshtml`
- [ ] T026 [US4] 實作上架時間 (CreatedTime) 排序邏輯在 `src/Services/ProductService.cs`

---

## Final Phase: Polish & Cross-Cutting Concerns

**目的**: 優化體驗、效能與日誌。

- [ ] T027 [P] 加入 `ILogger` 審計日誌記錄搜尋字串與處理時間在 `src/Services/ProductService.cs`
- [ ] T028 [P] 實作商品詳情頁 (Detail View) 在 `src/Pages/Products/Detail.cshtml`
- [ ] T029 優化搜尋索引效能，驗證 10k 資料量下的響應時間
- [ ] T030 執行 `quickstart.md` 中的驗收情境驗證

---

## Dependencies & Execution Order

### Phase Dependencies
- **Phase 1 & 2**: 必須先完成，因為後續 User Story 依賴資料模型與 DbContext。
- **User Stories (Phase 3-6)**: 
  - US1 (分頁) 是所有功能的載體，應最先完成。
  - US2, US3, US4 可以在 US1 完成後平行開發，但建議依優先級 (P1 -> P2) 執行。

### Parallel Opportunities
- T005, T006 (實體模型) 可並行。
- T009, T010, T011 (工具類與介面) 可並行。
- 一旦 Phase 3 (US1) 的 `ProductService` 結構確定，US2-US4 的邏輯實作可部分並行。

---

## Implementation Strategy

### MVP First (US1)
1. 完成基礎建設 (Phase 1 & 2)。
2. 實作 US1 並產生種子資料。
3. **驗證**: 確保 `/Products` 頁面能正常顯示分頁。

### Incremental Delivery
1. 基礎框架完成 -> 交付開發環境。
2. US1 完成 -> 交付基礎瀏覽功能 (MVP)。
3. US2 完成 -> 交付搜尋功能。
4. US3 & US4 完成 -> 交付進階過濾與排序功能。

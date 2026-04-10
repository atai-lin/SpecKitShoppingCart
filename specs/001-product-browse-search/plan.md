# Implementation Plan: 商品瀏覽與搜尋 (Product Browse & Search)

**Branch**: `001-product-browse-search` | **Date**: 2026-04-11 | **Spec**: [specs/001-product-browse-search/spec.md](spec.md)
**Input**: EF Core 查詢支援關鍵字完全比對、分類篩選、價格/上架時間排序，分頁每頁 20 筆，Razor Pages 渲染商品列表與詳情頁。

## Summary

實作電子商務網站的核心商品瀏覽功能。技術上採用 ASP.NET Core 10 Razor Pages 配合 EF Core Code First。核心需求包含支援精確關鍵字比對的搜尋、多維度排序（價格與上架時間）、分類過濾，以及每頁 20 筆的分頁機制。

## Technical Context

**Language/Version**: C# / ASP.NET Core 10
**Primary Dependencies**: EF Core, Razor Pages, Tailwind CSS, ILogger
**Storage**: SQL Server (via EF Core Code First)
**Testing**: xUnit/Moq, 80% coverage target
**Target Platform**: Web (Browsers)
**Project Type**: ASP.NET Core Web Application (Razor Pages)
**Performance Goals**: API/Page Load < 200ms
**Constraints**: Mandatory HTTPS, Shared Layout (`_Layout.cshtml`), ILogger for audit
**Scale/Scope**: 預計支援萬級商品量 (10k+ Products)，搜尋需在 1s 內完成

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

*   [x] ASP.NET Core 10 & EF Core Code First (Principle I)
*   [ ] JWT Auth (60min) & bcrypt Encryption (Principle II) - *Note: Not required for browse/search, but will be used for future cart/order features*
*   [x] Performance: API < 200ms & Shared Layout (Principle III)
*   [x] Testing: 80% Coverage target & C# Conventions (Principle IV)
*   [x] Observability: ILogger usage & Audit logging (Principle V)

## Project Structure

### Documentation (this feature)

```text
specs/001-product-browse-search/
├── spec.md              # Original requirement
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output (PageModel input/output)
└── tasks.md             # Phase 2 output (via /speckit.tasks)
```

### Source Code (repository root)

```text
src/
├── Models/              # EF Core Entities (Product, Category)
├── Data/                # DbContext, Migrations
├── Services/            # IProductService, ProductService (Business Logic)
└── Pages/               # Razor Pages
    ├── Products/
    │   ├── Index.cshtml # List view
    │   └── Detail.cshtml# Detail view
    └── Shared/          # _Layout.cshtml

tests/
├── UnitTests/           # Service layer tests
└── IntegrationTests/    # DbContext and PageModel tests
```

**Structure Decision**: 採用標準 Razor Pages 專案結構，業務邏輯抽離至 `Services` 層以符合憲法 Principle IV。

## Complexity Tracking

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| None | N/A | N/A |

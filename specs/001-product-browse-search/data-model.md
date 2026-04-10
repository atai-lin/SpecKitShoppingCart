# Data Model: 商品與分類

## 實體定義 (Entities)

### 1. Product (商品)
| 欄位 | 型別 | 說明 | 備註 |
| --- | --- | --- | --- |
| Id | int | 唯一識別碼 | PK |
| Name | string | 商品名稱 | 搜尋核心欄位 |
| Description| string | 商品描述 | |
| Price | decimal | 商品價格 | 需精確至兩位小數 |
| SalesVolume| int | 銷售量 | 支援「熱銷」排序 |
| ImageUrl | string | 圖片網址 | |
| CreatedTime | DateTime | 上架時間 | 支援「最新上架」排序 |
| CategoryId | int | 所屬分類 ID | FK |

### 2. Category (分類)
| 欄位 | 型別 | 說明 | 備註 |
| --- | --- | --- | --- |
| Id | int | 唯一識別碼 | PK |
| Name | string | 分類名稱 | |
| Description| string | 分類描述 | |

## 關係 (Relationships)
- **Product** - **Category**: Many-to-One (多對一)。一個商品僅能屬於一個分類。

## 索引優化 (Indexing)
- **Product.Name**: 建立索引，加速搜尋。
- **Product.CreatedTime**: 建立索引，加速排序。
- **Product.Price**: 建立索引，加速排序。

# Interface Contract: 商品列表頁 (Product Browse)

**功能**: 提供分頁商品清單、搜尋與過濾。
**端點**: `/Products/Index` (GET)

## 輸入參數 (Input Parameters)
| 參數名稱 | 型別 | 說明 | 範例 |
| --- | --- | --- | --- |
| `q` | string | 搜尋關鍵字 | "電競滑鼠" |
| `categoryIds`| int[] | 分類 ID 陣列 | `[1, 2]` |
| `sortBy` | string | 排序欄位 | "Price", "CreatedTime", "SalesVolume" |
| `isDesc` | bool | 是否降冪 | `true` |
| `pageIndex` | int | 當前頁碼 (預設 1) | `1` |

## 輸出屬性 (Output Properties - PageModel)
| 屬性名稱 | 型別 | 說明 |
| --- | --- | --- |
| `Products` | `PaginatedList<ProductDto>` | 商品 DTO 列表 |
| `Categories` | `IEnumerable<CategoryDto>` | 用於側欄選單的分類清單 |
| `CurrentQuery` | string | 當前搜尋條件 (回顯 UI) |
| `CurrentSort` | string | 當前排序欄位 |
| `CurrentCategoryIds` | int[] | 當前選中分類 |

## 關鍵 DTO 定義
```csharp
public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int SalesVolume,
    string ImageUrl,
    DateTime CreatedTime,
    string CategoryName
);
```

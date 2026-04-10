# Interface Contract: Refined Product Browse (Atelier Style)

**Function**: Provides a refined product listing with advanced filtering for Material and Color, aligned with the "Atelier" design.
**Endpoint**: `/Products/Index` (GET)

## Input Parameters (Input Parameters)
| Parameter Name | Type | Description | Example |
| --- | --- | --- | --- |
| `q` | string | Search keyword | "Cashmere" |
| `categoryIds`| int[] | Array of category IDs | `[1, 2]` |
| `material` | string | Filter by material | "Organic Wool" |
| `color` | string | Filter by color (hex or name) | "#323233" |
| `sortBy` | string | Sort field | "Price", "CreatedTime", "SalesVolume" |
| `isDesc` | bool | Descending order | `true` |
| `pageIndex` | int | Current page (default 1) | `1` |

## Output Properties (Output Properties - PageModel)
| Property Name | Type | Description |
| --- | --- | --- |
| `Products` | `PaginatedList<ProductDto>` | Paginated product DTO list |
| `Categories` | `IEnumerable<CategoryDto>` | Categories for the sidebar |
| `Materials` | `IEnumerable<string>` | Available material options for filtering |
| `Colors` | `IEnumerable<string>` | Available color options for filtering |
| `CurrentQuery` | string | Current query state |
| `CurrentSort` | string | Current sort field |
| `CurrentCategoryIds` | int[] | Selected category IDs |
| `CurrentMaterial` | string | Selected material filter |
| `CurrentColor` | string | Selected color filter |

## Refined DTO Definition
```csharp
public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int SalesVolume,
    string ImageUrl,
    DateTime CreatedTime,
    string CategoryName,
    string Material, // New attribute
    string Color     // New attribute
);
```

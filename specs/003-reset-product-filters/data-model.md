# Data Model: Product Filtering

## Entities

### FilterRequest (DTO)
Represents the incoming filter parameters from the frontend.
- `Categories`: `List<int>` - List of selected category IDs.
- `Materials`: `List<string>` - List of selected materials.
- `Colors`: `List<string>` - List of selected colors.
- `SearchTerm`: `string` - Optional search keyword.
- `SortOrder`: `string` - Sorting preference (e.g., "price_asc").
- `Page`: `int` - Page number for pagination.

### FilteredResult (DTO)
The response from the API, containing the data needed to update the UI.
- `Products`: `List<ProductDto>` - List of products matching the current filters.
- `Facets`: `FacetCounts` - Dynamic counts for each filter option.
- `Pagination`: `PaginationInfo` - Current page, total pages, etc.

### FacetCounts (DTO)
Contains the dynamic counts for all filterable attributes.
- `CategoryCounts`: `Dictionary<int, int>` (Category ID -> Count)
- `MaterialCounts`: `Dictionary<string, int>` (Material Name -> Count)
- `ColorCounts`: `Dictionary<string, int>` (Color Name -> Count)

## Validation Rules
- **Non-Empty**: Filter collections should be initialized as empty lists rather than null.
- **Sanitization**: Search term must be trimmed and sanitized before processing.
- **Range**: Page number must be > 0.

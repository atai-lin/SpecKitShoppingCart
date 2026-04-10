# API Contract: Product Filtering

## Endpoint: GET /api/products

Retrieves a filtered and paginated list of products based on the provided criteria.

### Query Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `categories` | List<int> | Optional. IDs of categories to filter by. |
| `materials` | List<string> | Optional. Material names to filter by. |
| `colors` | List<string> | Optional. Color names to filter by. |
| `q` | string | Optional. Search keyword. |
| `sort` | string | Optional. Sort order (e.g., 'price_asc', 'price_desc'). |
| `page` | int | Optional. Page number (default: 1). |

### Success Response (200 OK)

```json
{
  "products": [
    {
      "id": 1,
      "name": "Smartphone",
      "price": 599.99,
      "categoryName": "Electronics",
      "color": "Black",
      "material": "Metal"
    }
  ],
  "facets": {
    "categoryCounts": { "1": 10, "2": 5 },
    "materialCounts": { "Metal": 8, "Plastic": 7 },
    "colorCounts": { "Black": 12, "White": 3 }
  },
  "pagination": {
    "currentPage": 1,
    "totalPages": 5,
    "pageSize": 20,
    "totalCount": 95
  }
}
```

### Error Responses

- **400 Bad Request**: Invalid parameters or out-of-range page number.
- **500 Internal Server Error**: Unexpected server-side error.

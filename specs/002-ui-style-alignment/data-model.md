# Data Model: UI Style Alignment Extension

## Updated Entities

### Product (Extended)

| Field | Type | Description | Constraints |
| --- | --- | --- | --- |
| `Id` | int | Primary Key | Required |
| `Name` | string | Product name | Required, Max 200 |
| `Description` | string | Product description | Optional |
| `Price` | decimal | Product price | Required |
| `SalesVolume`| int | Number of sales | Required |
| `ImageUrl` | string | URL for the product image | Optional |
| `CreatedTime` | DateTime | Timestamp for latest sort | Required |
| `CategoryId` | int | Foreign Key to Category | Required |
| `Material` | string | Product material (new) | Optional |
| `Color` | string | Product color (hex/name) | Optional |

## Relationships

- **Product belongs to Category**: Each product must have a valid category ID.
- **Product Filtered by Material/Color**: These are standalone attributes used for filtering and presentation.

## Migration Plan (Code First)

1.  Add `Material` and `Color` properties to the `Product` entity in `src/Models/Product.cs`.
2.  Run `dotnet ef migrations add AddMaterialAndColorToProduct`.
3.  Run `dotnet ef database update`.
4.  Update `DbInitializer.cs` to seed some products with material and color data matching the Figma design (e.g., "Organic Wool", "Cashmere", "Italian Poplin").

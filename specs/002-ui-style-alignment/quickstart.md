# Quickstart: UI Style Alignment (Atelier Design)

## Prerequisites

- .NET 10 SDK
- Node.js & npm (for Tailwind CSS compilation)

## Setup and Development

1.  **Tailwind Configuration**:
    - Update `src/tailwind.config.js` to include the custom theme (colors and fonts).
    - Add Google Fonts link to `src/Pages/Shared/_Layout.cshtml`.

2.  **Database Update**:
    ```bash
    dotnet ef migrations add AddMaterialAndColorToProduct
    dotnet ef database update
    ```

3.  **Run Application**:
    ```bash
    dotnet run --project src/ShoppingCart.csproj
    ```

4.  **Tailwind Watch Mode** (optional, for development):
    ```bash
    npx tailwindcss -i src/wwwroot/css/site.css -o src/wwwroot/css/site.min.css --watch
    ```

## Verification Steps

1.  **Visual Alignment**:
    - Navigate to `/Products/Index`.
    - Compare header, hero section, and product grid against the Figma design at [Figma Design](https://www.figma.com/design/PwJ4BXEvxpy85XlGyVHMHW/Untitled?node-id=0-3&t=mTvmgSVzMYWKH5Iu-4).

2.  **Functional Filters**:
    - Select "Material" from the sidebar and verify only matching products appear.
    - Select a "Color" swatch and verify the list is filtered.
    - Check the "Load More Essentials" button at the bottom of the grid.

3.  **Responsive Layout**:
    - Resize browser to check tablet and mobile views. The sidebar should collapse or stack.

## Testing

```bash
dotnet test
```
Ensure all existing tests pass and new unit/integration tests for filtering logic are covered.

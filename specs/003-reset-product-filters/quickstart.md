# Quickstart: Product Filter Reset

## Setup

1. **Backend**: Add a new `ProductsController` to the `Controllers/` directory.
   - Implement the `GET /api/products` endpoint using EF Core to filter the database.
   - Use `AsNoTracking()` for performance.
2. **Frontend**: Create a `wwwroot/js/product-filter.js` script.
   - Attach click handlers to all filter items in the sidebar.
   - Update the `filterState` and trigger an AJAX call when a filter is toggled.
   - Use `history.pushState` to keep the URL in sync.

## Verification

### Manual Test
1. Load the product list page.
2. Click a category (e.g., "Electronics").
3. Verify the URL is updated and the list is filtered.
4. Click the same category again.
5. Verify the filter is removed and the original list returns.

### Automated Test
- Run `tests/integration/ProductsControllerTests.cs` to verify API filtering logic.
- Use a browser-based test (e.g., Playwright or Selenium) to verify the AJAX and History API behavior.

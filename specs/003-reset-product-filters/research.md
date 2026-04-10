# Research: Product Filter Reset Implementation

## Decisions

### 1. Web API Backend
- **Decision**: Use ASP.NET Core Web API controllers to expose a `/api/products` endpoint that accepts complex filter parameters.
- **Rationale**: Provides a clean separation of concerns and facilitates AJAX communication from the frontend.
- **Alternatives considered**: 
  - Using Razor Pages `OnGet` handlers with partial views: Rejected because it makes reusing the API for other clients (e.g., mobile) more difficult.

### 2. Frontend AJAX & History API
- **Decision**: Use the `Fetch API` for requests and the `History.pushState()` method to update the URL without page reloads.
- **Rationale**: Modern, native browser support without external dependencies (though jQuery can be used if already in the project). Keeps the URL shareable and back-button functional.
- **Alternatives considered**: 
  - Full page reloads: Rejected per user specification for "silent update".
  - Hash-based routing: Rejected as less modern and "clean" than the History API.

### 3. State Management
- **Decision**: Maintain a central `filterState` object in JavaScript that is synchronized with both the UI and the URL parameters.
- **Rationale**: Simplifies the logic for toggling filters and ensures the UI always matches the current effective search criteria.
- **Alternatives considered**: 
  - Reading state from DOM attributes each time: Rejected as error-prone and slow.

### 4. Real-time Count Updates
- **Decision**: The API will return not just the filtered products, but also updated counts for all available filter facets based on the *current* filter state (excluding the group itself for "what-if" counts).
- **Rationale**: Provides the "Dynamic Filter Counts" required by the specification (FR-007).

## Best Practices

- **Performance**: Use EF Core `AsNoTracking()` for read-only filter queries to hit the < 200ms latency target.
- **Security**: Ensure all input parameters are validated/sanitized to prevent injection, even though EF Core handles parameterized queries.
- **UX**: Show a loading indicator (e.g., skeleton or spinner) while AJAX requests are in flight.

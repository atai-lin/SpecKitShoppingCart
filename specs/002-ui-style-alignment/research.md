# Research: UI Style Alignment (Atelier Design)

## Decision: Typography Integration
**Choice**: Use Google Fonts for "Manrope" and "Work Sans".
**Rationale**: These fonts are available via Google Fonts, providing easy integration without hosting local files. They match the Figma design exactly.
**Alternatives Considered**: Local font hosting (more complex setup, no immediate benefit for this project).

## Decision: Asymmetrical Grid Layout (Visual Tension)
**Choice**: Use Tailwind's `grid` with custom `margin-top` or `translate-y` for specific items.
**Rationale**: The Figma design shows the 2nd and 6th items in a 3-column grid are offset. This can be achieved using `:nth-child` variants in Tailwind (e.g., `[&>div:nth-child(3n+2)]:mt-12`).
**Alternatives Considered**: Standard flexbox with margins (less maintainable for a grid).

## Decision: Data Model Extension & Filtering
**Choice**: Add `Material` (string) and `Color` (string/hex) to the `Product` model. Update `IProductService` and `ProductService` to include these as filter parameters.
**Rationale**: This allows the UI to filter products by these new attributes as required by the spec.
**Alternatives Considered**: Creating separate `Material` and `Color` entities (overkill for this simple prototype, strings are sufficient).

## Decision: Sticky Header with Backdrop Blur
**Choice**: Use Tailwind's `sticky`, `top-0`, `backdrop-blur`, and `bg-opacity`.
**Rationale**: This directly implements the Figma requirement for a sticky, semi-transparent, blurred navigation bar.
**Alternatives Considered**: Custom CSS (Tailwind classes are more concise).

## Decision: Color Palette Configuration
**Choice**: Extend the Tailwind theme with custom colors matching the design.
**Rationale**: Ensuring consistent use of `#fcf9f8`, `#323233`, `#5f5f5f`, and `#536254` across the site.
**Alternatives Considered**: Hardcoded hex values in HTML (not maintainable).

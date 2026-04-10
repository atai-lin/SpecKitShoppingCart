using System.Collections.Generic;

namespace ShoppingCart.Models.Dtos
{
    public class FilterRequest
    {
        public List<int> Categories { get; set; } = new List<int>();
        public List<string> Materials { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public string? Q { get; set; }
        public string? Sort { get; set; }
        public int Page { get; set; } = 1;
    }

    public class FilteredResult
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public FacetCounts Facets { get; set; } = new FacetCounts();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    }

    public class FacetCounts
    {
        public Dictionary<string, int> CategoryCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaterialCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ColorCounts { get; set; } = new Dictionary<string, int>();
    }

    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}

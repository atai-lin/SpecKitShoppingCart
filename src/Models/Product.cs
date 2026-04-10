using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int SalesVolume { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedTime { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public string? Material { get; set; }
        public string? Color { get; set; }
    }
}

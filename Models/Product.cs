using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? OldPrice { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public int ReviewCount { get; set; }

    [StringLength(50)]
    public string? Weight { get; set; }

    [StringLength(100)]
    public string? ProductType { get; set; }

    [StringLength(100)]
    public string? Flavor { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;
}

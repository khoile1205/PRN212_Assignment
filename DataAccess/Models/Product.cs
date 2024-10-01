using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? Type { get; set; }

    public int? Stock { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedTimestamp { get; set; }

    public DateTime? UpdatedTimestamp { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}

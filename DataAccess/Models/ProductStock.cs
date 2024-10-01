using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class ProductStock
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? Amount { get; set; }

    public decimal? Price { get; set; }

    public DateTime? CreatedTimestamp { get; set; }

    public DateTime? UpdatedTimestamp { get; set; }

    public virtual Product? Product { get; set; }
}

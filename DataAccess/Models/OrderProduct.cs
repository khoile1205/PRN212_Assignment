using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class OrderProduct
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedTimestamp { get; set; }

    public DateTime? UpdatedTimestamp { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}

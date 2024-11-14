using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Order
{
    public int Id { get; set; }

    public decimal TotalOrderPrice { get; set; }

    public decimal CustomerPay { get; set; }

    public string? Status { get; set; }

    public string? Reason { get; set; }

    public DateTime? CreatedTimestamp { get; set; }

    public DateTime? UpdatedTimestamp { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();

    public decimal Change => TotalOrderPrice - CustomerPay;

}

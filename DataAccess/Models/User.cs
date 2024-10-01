using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public string? Avatar { get; set; }

    public DateTime? CreatedTimestamp { get; set; }

    public DateTime? UpdatedTimestamp { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
}

using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class UserOrder
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? OrderId { get; set; }

    public DateTime? CreatedTimestamp { get; set; }

    public DateTime? UpdatedTimestamp { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }
}

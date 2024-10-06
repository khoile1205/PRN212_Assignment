using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class UserDto
    {
        [NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Status { get; set; }

        public string? Avatar { get; set; }

        public DateTime CreatedTimestamp { get; set; }

        public DateTime? UpdatedTimestamp { get; set; }

        public Role Role { get; set; }
    }
}

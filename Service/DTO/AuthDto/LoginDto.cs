using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class LoginDto
    {
        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Password { get; set; }

    }
}

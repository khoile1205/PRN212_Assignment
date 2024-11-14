using DataAccess.Models;
using Service.DTO.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.ProductDTO
{
    public class ProducDto
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

        public virtual ICollection<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>();

        public virtual ICollection<ProductStockDto> ProductStocks { get; set; } = new List<ProductStockDto>();
    }
}
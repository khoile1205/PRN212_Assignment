using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.ProductDTO
{
    public class ProductStockDto
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public int? Amount { get; set; }

        public decimal? Price { get; set; }

        public DateTime? CreatedTimestamp { get; set; }

        public DateTime? UpdatedTimestamp { get; set; }

        public virtual ProducDto? Product { get; set; }
    }
}
using SwiftPos.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Dto
{
    public class SaleDTO
    {
        public string SaleId { get; set; }
        public string CashierId { get; set; }
        public DateTime Date { get; set; }
        public List<SaleProductDTO> Products { get; set; } = new List<SaleProductDTO>();
        public SaleStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

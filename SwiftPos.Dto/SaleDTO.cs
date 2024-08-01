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
        [Required]
        public string SaleId { get; set; }  

        [Required]
        public string CashierId { get; set; }  

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Status { get; set; }  

        [Required]
        public List<ProductItemDTO> Products { get; set; } = new List<ProductItemDTO>();
    }
}

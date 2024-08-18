using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Dto
{
    public class UpdateQuantityDTO
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

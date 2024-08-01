using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftPos.Model
{
    public class Category
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Category() { }

        public Category(string name)
        {
            Id = Guid.NewGuid().ToString();  
            Name = name;
        }
    }
}

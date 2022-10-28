using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }        
        public string Name { get; set; }                 // required, plain text, max-length = 50
        public string Description { get; set; }          // optional, can contain html
        public string Image { get; set; }                // optional, url
        public Category Category { get; set; }           // required - one item can belong to only one category
        public decimal Price { get; set; }               // required, money
        public uint Amount { get; set; }                 // required, uint
    }
}

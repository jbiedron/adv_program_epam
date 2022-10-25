using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }                // required, plain text, max-length=50
        public string Image { get; set; }               // optional, url
        public int? ParentId { get; set; }              // optional
        public Category Parent { get; set; }
        public List<Category> Children { get; set; }
    }
}

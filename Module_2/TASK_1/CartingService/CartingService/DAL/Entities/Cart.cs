using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL.Entities
{
    public class Cart // : BaseEntity
    {
        public Cart(string externalId) 
        { 
            this.ExternalId = externalId;
            Items = new List<CartItem>();
        }

        public string ExternalId { get; set; }                // we need public setter for serialization/deserialization
        public List<CartItem> Items { get; set; }
    }
}

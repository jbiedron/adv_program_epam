using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.App.Products.Dto
{
    public class ProductDto
    {
        public int ProductId { get; private set; }
        public string Name { get; private set; }                 
        public string Description { get; private set; }          
        public string Image { get; private set; }                
        public Category Category { get; private set; }           
        public decimal Price { get; private set; }               
        public uint Amount { get; private set; }                 

        public ProductDto(Product entity) 
        { 
            this.ProductId = entity.ProductId;
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.Image = entity.Image;
            this.Category = entity.Category;
            this.Price = entity.Price;
            this.Amount = entity.Amount;
        }
    }
}

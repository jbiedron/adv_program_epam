using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.App.Categories.Dto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }                
        public string Image { get; set; }               
        public Category Parent { get; set; }
        public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();

        public CategoryDto(Category entity) 
        {
            this.CategoryId = entity.CategoryId;
            this.Name = entity.Name;
            this.Image = entity.Image;
            this.Parent = entity.Parent;

            if (entity.Children != null )
                entity.Children.ForEach(x => {
                    this.Children.Add(new CategoryDto(x));
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dummy
{
    public  class DummyProducts
    {
        public static List<Domain.Entities.Product> Get()
        {
            var results = new List<Domain.Entities.Product>() {
                new Domain.Entities.Product { ProductId = 1, Name = "PROD A" },
                new Domain.Entities.Product { ProductId = 2, Name = "PROD B" },
                new Domain.Entities.Product { ProductId = 3, Name = "PROD C" }
            };

            return results;
        }
    }
}

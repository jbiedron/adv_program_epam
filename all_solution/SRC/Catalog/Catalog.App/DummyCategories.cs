using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dummy
{
    public static class DummyCategories
    {
        public static List<Domain.Entities.Category> Get()
        {
            var results = new List<Domain.Entities.Category>() {
                new Domain.Entities.Category { CategoryId = 1, Name = "CAT A" },
                new Domain.Entities.Category { CategoryId = 2, Name = "CAT B" },
                new Domain.Entities.Category { CategoryId = 3, Name = "CAT C" }
            };

            return results;
        }
    }
}

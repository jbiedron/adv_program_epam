using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.App.Common
{
    public class PagedResponse<T> : Response<T>     // TODO: add interface ?
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }


        public PagedResponse(T data, int totalCount, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
           
            this.Data = data;
            this.Message = string.Empty;
            this.Succeeded = true;
            this.Errors = null;

            TotalPages = (int)Math.Floor((decimal)(totalCount / pageSize)) + 1;
        }
    }
}

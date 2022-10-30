using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.App.Common
{
    /*
    public interface PagedQueryRequest              // TODO: add interface?
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
    */

    public class PagedQueryRequest
    {
        public const int DEFAULT_PAGE_SIZE = 10;

        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

        public PagedQueryRequest() { 
            // nothing to do, required for request binding
        }

        public PagedQueryRequest(int pageNo, int pageSize)
        {
            this.PageNo = pageNo < 1 ? 1 : pageNo;
            this.PageSize = pageSize <= 0 ? DEFAULT_PAGE_SIZE : pageSize;      // default
        }
    }
}

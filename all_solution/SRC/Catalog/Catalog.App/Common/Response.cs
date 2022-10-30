using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.App.Common
{
    // TODO: add interface?!
    // TODO: move it to external library and reference from all the APIS?
    // This is arapper for API endpoints
    public class Response<T>
    {
        public Response()
        {
            // nothing to do
        }
        public Response(T data) 
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}

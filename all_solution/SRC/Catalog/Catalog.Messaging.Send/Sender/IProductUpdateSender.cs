using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Messaging.Send.Sender
{ 
    public interface IProductUpdateSender               // TODO: add the delegate/event
    {
        void SendProduct(Product product);
    }
}

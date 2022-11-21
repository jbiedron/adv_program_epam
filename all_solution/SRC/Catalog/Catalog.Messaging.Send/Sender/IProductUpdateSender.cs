﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Messaging.Send.Sender
{ 
    public interface IProductUpdateSender
    {
        void SendProduct(Product product);
    }
}

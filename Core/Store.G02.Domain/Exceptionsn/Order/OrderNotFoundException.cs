using Store.G02.Domain.Exceptionsn.NotFound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptionsn.Order
{
    public class OrderNotFoundException(Guid id) : NotFoundException($"This OrderId {id} was not Found")
    {
    }
}

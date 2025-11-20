using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptionsn.NotFound
{
    public class BasketNotFoundException(string id) : NotFoundException($"Basket with Key {id} was not found")
    {
    }
}

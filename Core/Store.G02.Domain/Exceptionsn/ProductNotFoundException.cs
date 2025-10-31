using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptionsn
{
    public  class ProductNotFoundException(int id ) : NotFoundException($"Product With Id {id} is Not Found")
    {
    }
}

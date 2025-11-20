using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptionsn.NotFound
{
    public abstract class NotFoundException(string message) : Exception(message)
    {

    }
}
